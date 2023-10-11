using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace Game.Core.GameSystems {
    public class UITimeManager : MonoBehaviour
    {
        [SerializeField] private float currentTime = 0;
        [SerializeField] private TextMeshProUGUI timeText;


        void Update()
        {
            currentTime += Time.deltaTime;

            var ts = TimeSpan.FromSeconds(currentTime);
            timeText.text = string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);

        }
    }
}
