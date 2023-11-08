using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Game.Core;

namespace Game {
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
                EventBus<SceneLoadedEvent>.AddListener(HandleSceneLoaded);
            }
        }
        public static GameStateManager instance;

        [Header("Refs")]
        //static ref to player
        public Agent player;
        public UIManager uiManager;

        [Header("Events")]
        public UnityEvent onStageComplete;

        //ref to advance object spawner
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
        private void HandleSceneLoaded(SceneLoadedEvent data)
        {
            uiManager.ObjectiveComplete = false;
        }

        //========= Handle Destroy ==========
        private void OnDestroy()
        {
            EventBus<SceneLoadedEvent>.RemoveListener(HandleSceneLoaded);
            //announce game end
            EventBus<GameEndEvent>.Invoke(new GameEndEvent());
        }
    }
}
