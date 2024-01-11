using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Core.GameSystems;

namespace Game {
    public class UIStepElement : MonoBehaviour
    {
        public TMP_Text label;
        public TMP_Text counterLabel;
        public Slider slider;

        private UIProgressBarHandler progressBarHandler;

        public void Initailize(UIProgressBarHandler barHandler, ObjectiveProgress progress)
        {
            progressBarHandler = barHandler;
            if (progress.useLargeBar)
            {
                progressBarHandler.Show(progress.label, progress.type == ObjectiveUIElementType.Counter);
            }
        }

        public void UpdateVisuals(ObjectiveProgress progress)
        {
            switch (progress.type)
            {
                case ObjectiveUIElementType.Description:
                    UpdateDescription(progress);
                    break;

                case ObjectiveUIElementType.Counter:
                    UpdateCounter(progress);
                    break;

                case ObjectiveUIElementType.ProgressBar:
                    UpdateSlider(progress);
                    break;
            }
        }

        private void UpdateDescription(ObjectiveProgress progress)
        {
            label.text = progress.label;
        }

        private void UpdateCounter(ObjectiveProgress progress)
        {
            label.text = progress.label;
            counterLabel.text = $"{progress.current} / {progress.total}";
            if (progress.useLargeBar)
            {
                progressBarHandler.UpdateCounter(progress.current, progress.total);
            }
        }

        private void UpdateSlider(ObjectiveProgress progress)
        {
            slider.value = progress.current / (float)progress.total;
            if (progress.useLargeBar)
            {
                progressBarHandler.UpdateProgress(progress.current / (float)progress.total);
            }
            //set label text
            label.text = $"{Mathf.FloorToInt(slider.value * 100f)}%";
        }
    }
}
