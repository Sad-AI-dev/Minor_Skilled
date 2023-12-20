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
        public RectTransform elementsHolder;

        [Header("Progress Refs")]
        public GameObject descriptionPrefab;
        public GameObject counterPrefab;
        public GameObject sliderPrefab;

        //vars
        private UIProgressBarHandler progressBarHandler;
        private bool usingBigBar;

        //manage created elements
        private List<UIStepElement> stepElements;

        public void Setup(ObjectiveStep step, UIProgressBarHandler progressBarHandler)
        {
            //initialize vars
            usingBigBar = false;
            this.progressBarHandler = progressBarHandler;
            stepElements = new List<UIStepElement>();
            //generate visuals
            GenerateVisuals(step.stepUISettings);
            //initialize values
            HandleStateChange(step);
        }

        //========= Generate Visuals ===========
        private void GenerateVisuals(StepUISettings settings)
        {
            //setup general visuals
            title.text = settings.title;
            //generate elements
            for (int i = 0; i < settings.progress.Count; i++) 
            {
                //big bar check
                if (!usingBigBar) { usingBigBar = settings.progress[i].useLargeBar; }
                //register created element
                stepElements.Add(GenerateElement(settings.progress[i]));
            }
        }

        private UIStepElement GenerateElement(ObjectiveProgress progress)
        {
            GameObject prefab = progress.type switch {
                ObjectiveUIElementType.Description => descriptionPrefab,
                ObjectiveUIElementType.Counter => counterPrefab,
                ObjectiveUIElementType.ProgressBar => sliderPrefab,
                _ => null,
            };
            //set default value
            UIStepElement stepElement = Instantiate(prefab, elementsHolder).GetComponent<UIStepElement>();
            stepElement.Initailize(progressBarHandler, progress);
            //return result
            return stepElement;
        }

        //=========== Handle State Change =========
        public void HandleStateChange(ObjectiveStep step)
        {
            if (step.state == ObjectiveState.InProgress)
            {
                //update visuals
                for (int i = 0; i < step.stepUISettings.progress.Count; i++)
                {
                    stepElements[i].UpdateVisuals(step.stepUISettings.progress[i]);
                }
            }
            else { HandleOnComplete(); } //objective step was completed
        }
        
        //=========== Handle On Complete ============
        private void HandleOnComplete()
        {
            //hide large bar if need
            if (usingBigBar) { progressBarHandler.Hide(); }
            //destroy elements
            foreach (Transform element in elementsHolder)
            {
                Destroy(element.gameObject);
            }
        }
    }
}
