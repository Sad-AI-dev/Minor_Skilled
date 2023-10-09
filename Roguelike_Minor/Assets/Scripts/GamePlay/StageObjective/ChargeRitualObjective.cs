using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using Game.Systems;

namespace Game {
    public class ChargeRitualObjective : MonoBehaviour
    {
        [Header("Timings")]
        [SerializeField] private float chargeSpeed;

        [Space(10f)]
        [SerializeField] private float dechargeDelay;
        [SerializeField] private float dechargeSpeed;

        [Header("UI Settings")]
        [SerializeField] private TMP_Text progressLabel;
        //[SerializeField] private Slider progressSlider;

        //vars
        private float progress;
        private bool isCharging;
        private bool done = false;

        //decharge vars
        private Coroutine dechargeDelayRoutine;
        private bool canDecharge;

        private void Update()
        {
            if (isCharging) { Charge(); }
            else if (canDecharge) { Decharge(); }
            progress = Mathf.Clamp(progress, 0, 100f);
            UpdateUI();
            //done check
            if (progress >= 100) { StopCharge(); }
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

        //=============== UI =============
        private void UpdateUI()
        {
            progressLabel.text = Mathf.FloorToInt(progress) + "%";
            //progressSlider.value = progress / 100.0f;
        }

        //============== Stop Charge ==========
        private void StopCharge()
        {
            GameStateManager.instance.HandleCompleteStageObject();
            Destroy(gameObject);
        }

        //============== Manage Triggers ==================
        private void OnTriggerEnter(Collider other)
        {
            if (!enabled) 
            {
                enabled = true;
                progress = 0;
            }
            isCharging = true;
            canDecharge = false;
            if (dechargeDelayRoutine != null) { StopCoroutine(dechargeDelayRoutine); }
        }

        private void OnTriggerExit(Collider other)
        {
            isCharging = false;
            dechargeDelayRoutine = StartCoroutine(DechargeDelayCo());
        }
    }
}
