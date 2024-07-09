using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.GameSystems {
    public class ObjectiveManager : MonoBehaviour
    {
        [Header("Stage Complete Settings")]
        public ObjectiveDataSO stageCompleteObjective;
        
        private List<Objective> objectives;

        public System.Action<Objective, ObjectiveStep> onStateChanged;
        public System.Action<Objective> onObjectiveCompleted;

        private void Awake()
        {
            objectives = new List<Objective>();
        }

        //============ Manage Objectives ============
        public void AddObjective(Objective objective)
        {
            objectives.Add(objective);
            objective.onStateChanged += OnStateChanged;
            objective.onCompletion += OnObjectiveCompleted;
            //create first step
            objective.CreateNextStep();
        }

        public void RemoveObjective(Objective objective)
        {
            objectives.Remove(objective);
        }

        public void Clear()
        {
            //hard reset list
            objectives.Clear();
        }

        //======= Handle State Change =========
        private void OnStateChanged(Objective objective, ObjectiveStep step)
        {
            onStateChanged?.Invoke(objective, step);
        }

        private void OnObjectiveCompleted(Objective objective)
        {
            onObjectiveCompleted?.Invoke(objective); //notify others of objective completion
            RemoveObjective(objective);
            //stage complete check
            if (AllObjectivesCompleted())
            {
                AddObjective(new Objective(stageCompleteObjective));
            }
        }

        private bool AllObjectivesCompleted()
        {
            return objectives.Count == 0;
        }
    }
}
