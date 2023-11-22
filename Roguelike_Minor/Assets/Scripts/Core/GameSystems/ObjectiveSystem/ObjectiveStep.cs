using System;
using UnityEngine;

namespace Game.Core.GameSystems {
    [Serializable]
    public enum ObjectiveState { InProgress, Done }

    public abstract class ObjectiveStep : MonoBehaviour
    {
        [HideInInspector] public Action<ObjectiveStep> onStateChanged;
        public ObjectiveState state;

        public virtual void ForceDestroy()
        {
            Destroy(gameObject);
        }
    }
}
