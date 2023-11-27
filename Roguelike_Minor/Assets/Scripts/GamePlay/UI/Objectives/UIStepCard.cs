using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Core.GameSystems;

namespace Game {
    [RequireComponent(typeof(RectTransform))]
    public class UIStepCard : MonoBehaviour
    {
        [Header("General Refs")]
        public TMP_Text title;
        public TMP_Text description;
        public GameObject checkMark;

        [Header("Progress Refs")]
        public GameObject counterHolder;
        public TMP_Text counterLabel;

        [Space(10f)]
        public GameObject sliderHolder;
        public TMP_Text sliderLabel;
        public Slider slider;

        //vars
        private bool done;
        private UIProgressBarHandler progressBarHandler;

        public void Setup(ObjectiveStep step, UIProgressBarHandler progressBarHandler)
        {
            done = false;
            StepUISettings settings = step.stepUISettings;
            //setup general data
            title.text = settings.title;
            description.text = settings.description;
            checkMark.SetActive(false);
            //setup progress bar
            if (settings.useLargeBar) {
                this.progressBarHandler = progressBarHandler;
                progressBarHandler.Show(settings.title, settings.type == ObjectiveType.Counter);
            }
            //setup progress data
            switch (settings.type)
            {
                case ObjectiveType.Checkmark: break; //do nothing
                case ObjectiveType.Counter:
                    counterHolder.SetActive(true);
                    UpdateCounter(settings);
                    break;

                case ObjectiveType.ProgressBar:
                    sliderHolder.SetActive(true);
                    UpdateSlider(settings);
                    break;
            }
            
        }

        //=========== Handle State Change =========
        public void HandleStateChange(ObjectiveStep step)
        {
            if (done) return; //don't update if step is completed
            //update values
            StepUISettings settings = step.stepUISettings;
            switch (settings.type)
            {
                case ObjectiveType.Checkmark: break; //do nothing
                case ObjectiveType.Counter:
                    UpdateCounter(settings);
                    break;

                case ObjectiveType.ProgressBar:
                    UpdateSlider(settings);
                    break;
            }
            //done check
            if (step.state == ObjectiveState.Done)
            {
                done = true;
                checkMark.SetActive(true);
                //hide progress bar if need
                if (settings.useLargeBar)
                {
                    progressBarHandler.Hide();
                }
            }
        }

        private void UpdateCounter(StepUISettings settings)
        {
            counterLabel.text = $"{settings.progressLabel}: {settings.currentCount}/{settings.maxCount}";
            if (settings.useLargeBar)
            {
                progressBarHandler.UpdateCounter(settings.currentCount, settings.maxCount);
            }
        }

        private void UpdateSlider(StepUISettings settings)
        {
            sliderLabel.text = $"{Mathf.FloorToInt(settings.progressPercent * 100)}%";
            slider.value = settings.progressPercent;
            if (settings.useLargeBar)
            {
                progressBarHandler.UpdateProgress(settings.progressPercent);
            }
        }
    }
}
