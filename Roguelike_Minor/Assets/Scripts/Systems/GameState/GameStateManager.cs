using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Game.Core;

namespace Game.Systems {
    public class GameStateManager : MonoBehaviour
    {
        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }
        }
        public static GameStateManager instance;

        [Header("Refs")]
        //static ref to player
        public Agent player;

        [Header("Events")]
        public UnityEvent onStageComplete;

        //ref to advane object spawner
        [HideInInspector] public AdvanceObjectSpawner advanceObjectSpawner;

        //========== Manage Stage State ==============
        public void HandleCompleteStageObject()
        {
            advanceObjectSpawner.SpawnAdvanceObject();
            onStageComplete?.Invoke();
        }
    }
}
