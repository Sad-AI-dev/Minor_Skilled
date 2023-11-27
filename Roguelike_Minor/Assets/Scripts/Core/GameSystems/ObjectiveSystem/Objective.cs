using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.GameSystems {
    [System.Serializable]
    public class Objective
    {
        public ObjectiveDataSO data;

        public Action<Objective, ObjectiveStep> onStateChanged;
        public Action<Objective> onCompletion;

        [Header("State Info")]
        public int currentStep;

        //vars
        [HideInInspector] public List<ObjectiveStep> steps;

        public Objective(ObjectiveDataSO data)
        {
            this.data = data;
            //set defaults
            currentStep = 0;
            steps = new List<ObjectiveStep>();
        }

        //========== Create Next Step ============
        private void HandleStepComplete()
        {
            currentStep++;
            //objective completed
            if (IsCompleted()) { HandleComplete(); }
            //create next step
            else { CreateNextStep(); }
        }
        private void HandleComplete()
        {
            //force destroy all steps
            for (int i = steps.Count - 1; i >= 0; i--)
            {
                if (steps[i]) { steps[i].ForceDestroy(); }
            }
            //notify completion
            onCompletion?.Invoke(this);
        }
        
        public void CreateNextStep()
        {
            ObjectiveStep step = data.ActivateStep(currentStep);
            step.objective = this;
            step.state = ObjectiveState.InProgress;
            step.onStateChanged += HandleStateChange;
            steps.Add(step);
            //notify change
            onStateChanged?.Invoke(this, step);
        }

        //========= Handle State Changes ============
        private void HandleStateChange(ObjectiveStep changedStep)
        {
            //notify objective manager
            onStateChanged?.Invoke(this, changedStep);
            
            if (changedStep.state == ObjectiveState.Done)
            { //destroy data object
                changedStep.OnCompleteStep();
                //advance to next step
                HandleStepComplete();
            }
        }

        //========= Completed check =============
        public bool IsCompleted()
        {
            return currentStep >= data.stepPrefabs.Count;
        }
    }
}
