using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.GameSystems {
    [Serializable]
    public enum ObjectiveState { InProgress, Done }
    public enum ObjectiveUIElementType { Description, Counter, ProgressBar }

    [System.Serializable]
    public class ObjectiveProgress
    {
        //counter
        public string label;
        public int total;
        public int current;
        //ui instructions
        public ObjectiveUIElementType type;
        public bool useLargeBar;
    }

    [System.Serializable]
    public class StepUISettings
    {
        [Header("General UI Settings")]
        public string title;

        [Header("Progress UI Settings")]
        public List<ObjectiveProgress> progress;
    }

    public abstract class ObjectiveStep : MonoBehaviour
    {
        [Header("Objective UI Settings")]
        public StepUISettings stepUISettings;

        [Header("Gameplay Data")]
        public ObjectiveState state;
        [HideInInspector] public Action<ObjectiveStep> onStateChanged;

        //refs
        [HideInInspector] public Objective objective;

        public virtual void OnCompleteStep()
        {
            Destroy(gameObject);
        }

        public virtual void ForceDestroy()
        {
            Destroy(gameObject);
        }
    }
}
