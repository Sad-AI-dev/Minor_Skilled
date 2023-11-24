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
        public TMP_Text progressLabel;
        public TMP_Text percentLabel;
        public TMP_Text counterLabel;

        //======== Manage State =======
        public void Show(string title, bool showCounter = false)
        {
            sliderHolder.SetActive(true);
            slider.value = 0;
            progressLabel.text = title;
            counterLabel.gameObject.SetActive(showCounter);
        }

        public void Hide()
        {
            sliderHolder.SetActive(false);
            progressLabel.text = "";
        }

        //======== Manage Progress ========
        public void UpdateProgress(float percent)
        {
            slider.value = percent * slider.maxValue;
            percentLabel.text = (Mathf.FloorToInt(percent * 100)) + "%";
        }
    }
}
