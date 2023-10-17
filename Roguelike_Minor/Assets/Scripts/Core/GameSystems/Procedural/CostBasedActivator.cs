using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Game.Util;

namespace Game.Core.GameSystems {
    public class CostBasedActivator : MonoBehaviour
    {
        [System.Serializable]
        public class Option : ISerializationCallbackReceiver {
            public string name;
            public int price;
            public UnityEvent onSelect;
            [HideInInspector] public bool initialized;

            public void OnBeforeSerialize() { }
            public void OnAfterDeserialize() {
                if (!initialized) {
                    price = 1;
                    initialized = true;
                }
            }
        }

        public enum ActivateMode {
            Manual, Interval
        }

        public enum BudgetSpikeMode {
            None, Random, Interval, Interval_Random
        }

        public enum SaveMode {
            None, Random, Interval, Interval_Random
        }

        [Header("Activation Settings")]
        [Tooltip("Determines when behavior is activated.\n\n" +
            "Manual: only activates when Activate() is called.\n" +
            "Interval: activates periodically.\n")]
        public ActivateMode activateMode;
        [Tooltip("Only used when 'Activate Mode' is set to 'Interval'.\n" +
            "Determines the interval in seconds.")]
        [HideIf(nameof(ActivateModeIsManual))]
        public float activateIntervalTime;
        [SerializeField] private bool activateOnStart;

        [Header("Budget Settings")]
        public int budget;
        [Space(10f)]
        //gain values
        public int budgetGain;
        public int gainRampup;
        public int gainRampupFrequency;

        [Header("Exponential Scaling Settings")]
        [SerializeField] private bool enableExponentialScaling;

        [HideIf(nameof(ExponentialScalingDisabled))]
        public int exponentialRampup;
        [HideIf(nameof(ExponentialScalingDisabled))]
        public int exponentialRampupFrequency;

        [Header("Budget Spike Settings")]
        [Tooltip("Determines how the spike behavior is activated.\n\n" +
            "None: spike behavior will never be activated.\n" +
            "Random: spike behavior will randomly be activated.\n" +
            "Interval: spike behavior will be activated once per set interval.\n" +
            "Interval_Random: similar to 'Interval', but interval time is randomly picked.")]
        public BudgetSpikeMode budgetSpikeMode;

        [HideIf(nameof(BudgetSpikeDisabled))]
        public float budgetSpikeMultiplier = 1f;
        [Space(10)]

        [HideIf(nameof(SpikeModeNotRandom))]
        [Range(0, 100)] public float budgetSpikeActivationChance;
        [HideIf(nameof(SpikeModeNotInterval))]
        public int budgetSpikeInterval;
        [HideIf(nameof(SpikeModeNotIntervalRandom))]
        public int budgetSpikeIntervalMin, budgetSpikeIntervalMax;

        [Header("Save Behavior Settings")]
        [Tooltip("Determines how the save behavior is activated.\n\n" +
            "None: save behavior will never be activated.\n" +
            "Random: save behavior will randomly be activated.\n" +
            "Interval: save behavior will be activated once per set interval.\n" +
            "Interval_Random: similar to 'Interval', but interval time is randomly picked.")]
        public SaveMode saveMode;

        [HideIf(nameof(SaveModeDisabled))]
        [Range(0f, 100f)] public float minSavePercent;
        [HideIf(nameof(SaveModeDisabled))]
        [Range(0f, 100f)] public float maxSavePercent;
        [Space(10)]

        [Tooltip("Only used when 'Save Mode' is set to 'Random'.\n" +
            "Determines the chance for the save behavior to activate, in percentage.")]
        [HideIf(nameof(SaveModeNotRandom))]
        [Range(0f, 100f)] public float saveChance;
        [Tooltip("Only used when 'Save Mode' is set to 'Interval'.\n" +
            "Determines after how many budget gains the save behavior should activate.")]
        [HideIf(nameof(SaveModeNotInterval))]
        public int saveInterval;
        [Tooltip("Only used when 'Save Mode' is set to 'Interval_Random'.")]
        [HideIf(nameof(SaveModeNotIntervalRandom))]
        public int minRandInterval;
        [Tooltip("Only used when 'Save Mode' is set to 'Interval_Random'.")]
        [HideIf(nameof(SaveModeNotIntervalRandom))]
        public int maxRandInterval;

        [Header("Options")]
        [SerializeField] private List<Option> options;

        [Header("Editor Options")]
        public int cyclesToSimulate = 5;

        //editor conditionals
        public bool ActivateModeIsManual => activateMode == ActivateMode.Manual;

        public bool ExponentialScalingDisabled => !enableExponentialScaling;

        public bool BudgetSpikeDisabled => budgetSpikeMode == BudgetSpikeMode.None;
        public bool SpikeModeNotRandom => budgetSpikeMode != BudgetSpikeMode.Random;
        public bool SpikeModeNotInterval => budgetSpikeMode != BudgetSpikeMode.Interval && budgetSpikeMode != BudgetSpikeMode.Interval_Random;
        public bool SpikeModeNotIntervalRandom => budgetSpikeMode != BudgetSpikeMode.Interval_Random;

        public bool SaveModeDisabled => saveMode == SaveMode.None;
        public bool SaveModeNotRandom => saveMode != SaveMode.Random;
        public bool SaveModeNotInterval => saveMode != SaveMode.Interval;
        public bool SaveModeNotIntervalRandom => saveMode != SaveMode.Interval_Random;

        //vars
        private int rampupCounter;
        //exponential scaling vars
        private int exponentialCounter;
        //spike vars
        private int spikeCounter;
        //save feature vars
        private int savedBudget;
        private int saveCounter;
        private bool stopReq;

        //external multiplier var
        [HideInInspector] public float externalMultiplier = 1f;

        //option decision vars
        private int minPrice;

        //editor pollish
        private float oldMinSpikeInterval;
        private float oldMinSave;
        private float oldMinInterval;

        private void Start()
        {
            ResetVars();
            minPrice = CalcMinPrice();
            if (saveMode == SaveMode.Interval_Random) { SetNewRandSaveInterval(); }
            if (activateOnStart) { Activate(); }
        }

        //================== activate ==================
        public void Activate(bool previewMode = false)
        {
            //step 1: purchace options
            if (!previewMode) PurchaseOptions(); //makes start budget more intuitive + allows for showing current budget in editor
            //step 2: gain budget
            GainBudget();
            //step 3: save behavior
            if (saveMode != SaveMode.None) { HandleSaveBehavior(); }
            //repeat?
            if (activateMode == ActivateMode.Interval && !previewMode) {
                StartCoroutine(ActivateIntervalCo());
            }
        }
        private IEnumerator ActivateIntervalCo()
        {
            yield return new WaitForSeconds(activateIntervalTime);
            if (!stopReq) { Activate(); }
            else { stopReq = false; } //reset stopReq
        }

        //=============== ForceActivate ===============
        public void ForceActivate(float multiplier = 1f)
        {
            //gain budget
            budget += Mathf.RoundToInt(budgetGain * multiplier);
            //puchase options
            PurchaseOptions();
        }

        //================== stop ==================
        public void Stop()
        {
            stopReq = true;
        }

        //=============== gain budget step ===============
        private void GainBudget()
        {
            budget += budgetGain;
            //spike feature
            HandleSpike();
            //external multiplier
            budget = Mathf.RoundToInt(budget * externalMultiplier);
            //save feature
            HandleSavedBudget();
            //scaling
            if (enableExponentialScaling) { HandleExponentialScaling(); } //increase rampup value BEFORE applying it
            HandleRampup();
        }

        private void HandleSavedBudget()
        {
            budget += savedBudget;
            savedBudget = 0;
        }

        private void HandleExponentialScaling()
        {
            exponentialCounter++;
            if (exponentialCounter >= exponentialRampupFrequency) {
                gainRampup += exponentialRampup;
                exponentialCounter = 0;
            }
        }

        private void HandleRampup()
        {
            rampupCounter++;
            if (rampupCounter >= gainRampupFrequency) {
                budgetGain += gainRampup;
                rampupCounter = 0;
            }
        }

        //================== spike feature ==================
        private void HandleSpike()
        {
            switch (budgetSpikeMode) {
                default: break; //do nothing by default
                case BudgetSpikeMode.Random:
                    HandleRandomSpike();
                    break;
                case BudgetSpikeMode.Interval:
                case BudgetSpikeMode.Interval_Random:
                    HandleIntervalSpike();
                    break;
            }
        }

        private void HandleRandomSpike()
        {
            if (Random.Range(0f, 100f) > budgetSpikeActivationChance) {
                SpikeBudget();
            }
        }

        private void HandleIntervalSpike()
        {
            spikeCounter++;
            if (spikeCounter >= budgetSpikeInterval) {
                SpikeBudget();
                spikeCounter = 0;
                if (budgetSpikeMode == BudgetSpikeMode.Interval_Random) {
                    budgetSpikeInterval = Random.Range(budgetSpikeIntervalMin, budgetSpikeIntervalMax + 1);
                }
            }
        }

        private void SpikeBudget()
        {
            budget += Mathf.RoundToInt(budgetGain * budgetSpikeMultiplier);
        }

        //================== save step ==================
        private void HandleSaveBehavior()
        {
            switch (saveMode) {
                case SaveMode.Random:
                    HandleRandomSave(); break;

                case SaveMode.Interval:
                case SaveMode.Interval_Random:
                    HandleIntervalSave(); break;
            }
        }

        private void HandleRandomSave()
        {
            if (Random.Range(0.0f, 100.0f) < saveChance) {
                SaveBudget();
            }
        }

        private void HandleIntervalSave()
        {
            saveCounter++;
            if (saveCounter >= saveInterval) {
                if (saveMode == SaveMode.Interval_Random) { SetNewRandSaveInterval(); }
                SaveBudget();
                saveCounter = 0;
            }
        }
        private void SetNewRandSaveInterval()
        {
            saveInterval = Random.Range(minRandInterval, maxRandInterval + 1);
        }

        private void SaveBudget()
        {
            int toSave = Mathf.RoundToInt((Random.Range(minSavePercent, maxSavePercent) / 100) * budget);
            savedBudget = toSave;
            budget -= toSave;
        }

        //=============== purchase step ===============
        private void PurchaseOptions()
        {
            while (budget >= minPrice) {
                List<Option> availableOptions = GetAvailableOptions();
                Option chosenOption = availableOptions[Random.Range(0, availableOptions.Count)];
                budget -= chosenOption.price; //pay
                chosenOption.onSelect?.Invoke();
            }
        }

        //========= price calcs =========
        private int CalcMinPrice()
        {
            int minPrice = options[0].price;
            for (int i = 1; i < options.Count - 1; i++) {
                if (options[i].price < minPrice) {
                    minPrice = options[i].price;
                }
            }
            return minPrice;
        }

        private List<Option> GetAvailableOptions()
        {
            List<Option> availables = new List<Option>();
            foreach (Option option in options) {
                if (option.price <= budget) {
                    availables.Add(option);
                }
            }
            return availables;
        }

        //================== manage options list ==================
        public void AddOption(Option option)
        {
            options.Add(option);
            minPrice = CalcMinPrice();
        }

        public bool RemoveOption(Option option)
        {
            if (options.Contains(option)) {
                options.Remove(option);
                minPrice = CalcMinPrice();
                return true;
            }
            return false;
        }

        //================== Reset ==================
        public void ResetVars()
        {
            rampupCounter = 0;
            exponentialCounter = 0;
            spikeCounter = 0;
            savedBudget = 0;
            saveCounter = 0;
            stopReq = false;
        }

        //===================== editor pollish =====================
        private void OnValidate()
        {
            SpikeIntervalCheck();
            SavePercentageCheck();
            SaveIntervalCheck();
            if (options != null) {
                ValidPricesCheck();
            }
        }

        //spike interval check
        private void SpikeIntervalCheck()
        {
            if (budgetSpikeIntervalMin > budgetSpikeIntervalMax) {
                if (budgetSpikeIntervalMin != oldMinSpikeInterval) {
                    budgetSpikeIntervalMax = budgetSpikeIntervalMin; //min moved, move max up to min
                }
                else {
                    budgetSpikeIntervalMin = budgetSpikeIntervalMax; //max moved, move min down to max
                }
            }
            //update old vars
            oldMinSpikeInterval = budgetSpikeIntervalMin;
        }

        //save percentage check
        private void SavePercentageCheck()
        {
            if (minSavePercent > maxSavePercent) {
                if (minSavePercent != oldMinSave) {
                    maxSavePercent = minSavePercent; //min moved, move max up to min
                }
                else {
                    minSavePercent = maxSavePercent; //max moved, move min down to max
                }
                //update old vars
                oldMinSave = minSavePercent;
            }
        }

        //save rand interval check
        private void SaveIntervalCheck()
        {
            if (minRandInterval > maxRandInterval) {
                if (minRandInterval != oldMinInterval) {
                    maxRandInterval = minRandInterval; //min moved, move max up to min
                }
                else {
                    minRandInterval = maxRandInterval; //max moved, move min down to max
                }
            }
            //update old vars
            oldMinInterval = minRandInterval;
        }

        //valid prices check
        private void ValidPricesCheck()
        {
            foreach (Option opt in options) {
                if (opt.price <= 0) {
                    Debug.LogWarning($"{transform.name}: option {opt.name} has an invalid price, please make sure price is higher than 0!");
                }
            }
        }
    }
}
