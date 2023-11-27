using System;
using UnityEngine;

namespace Game.Core.GameSystems {
    [Serializable]
    public enum ObjectiveState { InProgress, Done }
    public enum ObjectiveType { Checkmark, Counter, ProgressBar }

    [System.Serializable]
    public class StepUISettings
    {
        public string title;
        public string description;
        [Header("Progress UI Settings")]
        public bool useLargeBar;
        public ObjectiveType type;

        [Space(10f)]
        public string progressLabel;
        public int maxCount;
        public int currentCount;
        public float progressPercent; //0 to 1
    }

    public abstract class ObjectiveStep : MonoBehaviour
    {
        [Header("Objective UI Settings")]
        public StepUISettings stepUISettings;

        [Header("Gameplay Data")]
        public ObjectiveState state;
        [HideInInspector] public Action<ObjectiveStep> onStateChanged;

        public virtual void ForceDestroy()
        {
            Destroy(gameObject);
        }
    }
}
