using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.GameSystems {
    public class ObjectiveManager : MonoBehaviour
    {
        private List<Objective> objectives;

        public System.Action<Objective, ObjectiveStep> onStateChanged;
        public System.Action<ObjectiveManager, Objective> onObjectiveCompleted;

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
        public void OnStateChanged(Objective objective, ObjectiveStep step)
        {
            onStateChanged?.Invoke(objective, step);
        }

        public void OnObjectiveCompleted(Objective objective)
        {
            onObjectiveCompleted?.Invoke(this, objective); //notify others of objective completion
            RemoveObjective(objective);
            
        }

        public bool AllObjectivesCompleted()
        {
            return objectives.Count == 0;
        }
    }
}