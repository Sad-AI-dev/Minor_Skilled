using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core.GameSystems;

namespace Game {
    public class InteractStep : ObjectiveStep
    {
        public void CompleteStep(Interactor interactor)
        {
            state = ObjectiveState.Done;
            onStateChanged?.Invoke(this);
        }
    }
}
