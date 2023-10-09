using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
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
                SceneManager.sceneLoaded += HandleSceneLoad;
            }
        }
        public static GameStateManager instance;

        [Header("Refs")]
        //static ref to player
        public Agent player;
        public UIManager uiManager;

        [Header("Events")]
        public UnityEvent onStageComplete;

        //ref to advane object spawner
        [HideInInspector] public AdvanceObjectSpawner advanceObjectSpawner;

        //========== Manage Stage State ==============
        public void HandleCompleteStageObject()
        {
            advanceObjectSpawner.SpawnAdvanceObject();
            onStageComplete?.Invoke();
            //update UI manager
            uiManager.ObjectiveComplete = true;
        }

        //========= Handle Scene Load ========
        private void HandleSceneLoad(Scene scene, LoadSceneMode loadMode)
        {
            uiManager.ObjectiveComplete = false;
        }

        //========= Handle Destroy ==========
        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= HandleSceneLoad;
        }
    }
}
