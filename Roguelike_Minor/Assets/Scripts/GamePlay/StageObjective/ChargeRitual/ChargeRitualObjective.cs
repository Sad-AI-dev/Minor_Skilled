using System.Collections;
using UnityEngine;
using TMPro;
using Game.Core.GameSystems;
using Game.Core;

namespace Game {
    public class ChargeRitualObjective : ObjectiveStep
    {
        [Header("Timings")]
        [SerializeField] private float chargeSpeed;

        [Space(10f)]
        [SerializeField] private float dechargeDelay;
        [SerializeField] private float dechargeSpeed;

        [Header("Difficulty Settings")]
        [SerializeField] private float prewarmMultiplier; //multiplier for enemies spawned on objective start
        [SerializeField] private float spawnMultiplier; //multiplier for amount of enemies being spawned

        [Header("Visuals")]
        [SerializeField] private GameObject rangeIndicator;
        [SerializeField] private PickupMovement pillarMovement;
        [SerializeField] private AnimationCurve pillarSpeed;
        [SerializeField] private float maxPillarSpeed = 300f;

        [Header("UI Settings")]
        [SerializeField] private TMP_Text progressLabel;

        //vars
        private bool activated;
        private float progress;
        private bool isCharging;

        //decharge vars
        private Coroutine dechargeDelayRoutine;
        private bool canDecharge;

        //ref
        private UIProgressBarHandler progressBar;

        private void Awake()
        {
            EventBus<ObjectiveSpawned>.Invoke(new ObjectiveSpawned { objective = transform.parent.gameObject });
        }

        private void Start()
        {
            rangeIndicator.SetActive(false);
            enabled = false;
        }

        private void Update()
        {
            if (isCharging) { Charge(); }
            else if (canDecharge) { Decharge(); }
            else { return; } //objective is not active
            UpdatePillarSpeed();
            //update UI
            progress = Mathf.Clamp(progress, 0, 100f);
            UpdateUI();
            //done check
            if (progress >= 100) { StopCharge(); }
            else { onStateChanged?.Invoke(this); } //normal state objective
        }
        
        //=============== Start Charge ==============
        public void StartCharge()
        {
            activated = true;
            enabled = true;
            isCharging = true;
            progress = 0;
            rangeIndicator.SetActive(true);
            //up enemy spawning
            EnemySpawner.instance.ForceSpawn(prewarmMultiplier);
            EnemySpawner.instance.SetExternalSpawnMultiplier(spawnMultiplier);
            //UI
            progressBar = GameStateManager.instance.uiManager.progressBar;
            progressBar.Show();
        }

        //=============== Charge ===============
        private void Charge()
        {
            progress += chargeSpeed * Time.deltaTime;
        }

        //=============== Decharge ==============
        private IEnumerator DechargeDelayCo()
        {
            yield return new WaitForSeconds(dechargeDelay);
            canDecharge = true;
        }
        
        private void Decharge()
        {
            progress -= dechargeSpeed * Time.deltaTime;
        }

        //============ Pillar Visuals ===========
        private void UpdatePillarSpeed()
        {
            pillarMovement.rotateSpeed = pillarSpeed.Evaluate(progress / 100f) * maxPillarSpeed;
        }

        //=============== UI =============
        private void UpdateUI()
        {
            progressLabel.text = Mathf.FloorToInt(progress) + "%";
            progressBar.UpdateProgress(progress / 100f);
        }

        //============== Stop Charge ==========
        private void StopCharge()
        {
            EnemySpawner.instance.SetExternalSpawnMultiplier(-spawnMultiplier);
            GameStateManager.instance.HandleCompleteStageObject();
            //Update UI
            progressBar.Hide();
            //announce completion
            OnComplete();
        }

        private void OnComplete()
        {
            state = ObjectiveState.Done;
            onStateChanged?.Invoke(this);
        }

        //===== Handle Destroy ====
        public override void ForceDestroy()
        {
            Destroy(transform.parent.gameObject);
        }

        //============== Manage Triggers ==================
        private void OnTriggerEnter(Collider other)
        {
            if (!activated || !other.CompareTag("Player")) { return; } //only activate on player
            isCharging = true;
            canDecharge = false;
            if (dechargeDelayRoutine != null) { StopCoroutine(dechargeDelayRoutine); }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!activated || !other.CompareTag("Player")) { return; } //only activate on player
            isCharging = false;
            dechargeDelayRoutine = StartCoroutine(DechargeDelayCo());
        }
    }
}
