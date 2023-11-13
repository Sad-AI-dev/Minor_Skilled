using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Game {
    public class UIProgressBarHandler : MonoBehaviour
    {
        [Header("Refs")]
        public GameObject sliderHolder;
        public Slider slider;
        public TMP_Text percentLabel;

        //======== Manage State =======
        public void Show()
        {
            sliderHolder.SetActive(true);
            slider.value = 0;
        }

        public void Hide()
        {
            sliderHolder.SetActive(false);
        }

        //======== Manage Progress ========
        public void UpdateProgress(float percent)
        {
            slider.value = percent * slider.maxValue;
            percentLabel.text = (Mathf.FloorToInt(percent * 100)) + "%";
        }
    }
}
