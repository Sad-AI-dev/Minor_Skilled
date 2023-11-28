using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core.GameSystems;

namespace Game {
    public class InteractStep : ObjectiveStep
    {
        [Header("Destory Settings")]
        [SerializeField] private bool destroyOnComplete = true;

        public void CompleteStep(Interactor interactor)
        {
            state = ObjectiveState.Done;
            //update state
            onStateChanged?.Invoke(this);
        }

        public override void OnCompleteStep()
        {
            if (destroyOnComplete) { base.OnCompleteStep(); }
            else { GetComponent<Interactable>().enabled = false; }
        }
    }
}
