using System.Collections;
using UnityEngine;
using TMPro;
using Game.Core.GameSystems;

namespace Game {
    public class ChargeRitualObjective : ObjectiveStep
    {
        [Header("Timings")]
        [SerializeField] private float chargeSpeed;

        [Space(10f)]
        [SerializeField] private float dechargeDelay;
        [SerializeField] private float dechargeSpeed;

        [Header("Difficulty Settings")]
        [SerializeField] private float range;
        [SerializeField] private float prewarmMultiplier; //multiplier for enemies spawned on objective start
        [SerializeField] private float spawnMultiplier; //multiplier for amount of enemies being spawned

        [Header("Visuals")]
        [SerializeField] private Transform rangeIndicator;
        [SerializeField] private AnimationCurve pillarSpeed;
        [SerializeField] private float maxPillarSpeed = 300f;

        [Header("UI Settings")]
        [SerializeField] private TMP_Text progressLabel;

        //vars
        private float progress;
        private bool isCharging;

        //decharge vars
        private Coroutine dechargeDelayRoutine;
        private bool canDecharge;

        //external refs
        private PickupMovement pillarMovement;

        //=============== Start Charge ==============
        private void Start()
        {
            StartCharge();
        }

        private void StartCharge()
        {
            //setup range vars
            GetComponent<SphereCollider>().radius = range;
            rangeIndicator.localScale = Vector3.one * (range * 2);
            //set position
            ObjectiveStep lastStep = objective.steps[^2];
            transform.position = lastStep.transform.position;
            //setup pillar movement
            pillarMovement = lastStep.GetComponentInChildren<PickupMovement>();
            //setup base state vars
            isCharging = true;
            progress = 0;
            //up enemy spawning
            EnemySpawner.instance.ForceSpawn(prewarmMultiplier);
            EnemySpawner.instance.SetExternalSpawnMultiplier(spawnMultiplier);
        }

        //=============== Charge ==============
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
            stepUISettings.progressPercent = progress / 100f;
            progressLabel.text = Mathf.FloorToInt(progress) + "%";
        }

        //============== Stop Charge ==========
        private void StopCharge()
        {
            EnemySpawner.instance.SetExternalSpawnMultiplier(-spawnMultiplier);
            GameStateManager.instance.HandleCompleteStageObject();
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
            Destroy(gameObject);
        }

        //============== Manage Triggers ==================
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) { return; } //only activate on player
            isCharging = true;
            canDecharge = false;
            if (dechargeDelayRoutine != null) { StopCoroutine(dechargeDelayRoutine); }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) { return; } //only activate on player
            isCharging = false;
            dechargeDelayRoutine = StartCoroutine(DechargeDelayCo());
        }
    }
}
