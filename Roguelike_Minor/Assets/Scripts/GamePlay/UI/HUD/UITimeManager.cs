using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Game {
    public class UITimeManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timeText;

        //vars
        private float currentTime = 0;

        private void Start()
        {
            StartCoroutine(UpdateTimerCo());
        }

        private IEnumerator UpdateTimerCo()
        {
            //advance time
            if (!GameStateManager.instance.scalingIsPaused)
            {
                yield return null;
                currentTime += Time.deltaTime;
                UpdateLabel();
            }
            //wait until time is unpaused
            else { yield return new WaitWhile(() => GameStateManager.instance.scalingIsPaused); }
            //loop
            StartCoroutine(UpdateTimerCo());
        }

        private void UpdateLabel()
        {
            var ts = TimeSpan.FromSeconds(currentTime);
            timeText.text = string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);
        }
    }
}
