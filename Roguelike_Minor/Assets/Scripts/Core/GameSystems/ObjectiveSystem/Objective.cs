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

        public Objective(ObjectiveDataSO data)
        {
            this.data = data;
            //set defaults
            currentStep = 0;
        }

        //========== Create Next Step ============
        private void HandleStepComplete()
        {
            currentStep++;
            //objective completed
            if (IsCompleted()) { onCompletion?.Invoke(this); }
            //create next step
            else { CreateNextStep(); }
        }
        
        public void CreateNextStep()
        {
            ObjectiveStep step = data.ActivateStep(currentStep);
            step.state = ObjectiveState.InProgress;
            step.onStateChanged += HandleStateChange;
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
                changedStep.ForceDestroy();
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
