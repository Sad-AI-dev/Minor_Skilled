using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Game.Core;
using Game.Core.GameSystems;

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
                EventBus<ShopLoadedEvent>.AddListener(HandleShopLoad);
            }
        }
        public static GameStateManager instance;

        [System.Serializable] 
        public class ListWrapper<T> 
        { 
            public List<T> list;
            public T this[int index]
            {
                get { return list[index]; }
                set { list[index] = value; }
            }
            public int Count { get { return list.Count; } }
        }

        [Header("Refs")]
        //static ref to player
        public Agent player;
        public UIManager uiManager;
        [SerializeField] private SceneLoader sceneLoader;

        [Header("Events")]
        public UnityEvent onStageComplete;

        [Header("Planet Advancement Settings")]
        [SerializeField] private List<ListWrapper<int>> planetSceneIndeces;
        [SerializeField] private int shopIndex;

        //ref to advance object spawner
        [HideInInspector] public AdvanceObjectSpawner advanceObjectSpawner;
        [HideInInspector] public LootSpawner lootSpawner;

        //scene advancement vars
        public int CurrentStage { get; private set; } = 1;

        //paused state
        [HideInInspector] public bool scalingIsPaused;
        private bool isShopStage;

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
            //handle pause
            scalingIsPaused = isShopStage;
            isShopStage = false; //reset
        }

        //========= Handle Shop Load ========
        private void HandleShopLoad(ShopLoadedEvent data)
        {
            isShopStage = true;
        }

        //================== Handle Planet Advancement ================
        public void AdvanceToNextPlanet()
        {
            CurrentStage++;
            sceneLoader.LoadScene(GetNextStageBuildIndex());
        }
        private int GetNextStageBuildIndex()
        {
            int stageIndex = CurrentStage % planetSceneIndeces.Count;
            return planetSceneIndeces[stageIndex][Random.Range(0, planetSceneIndeces[stageIndex].Count)];
        }

        public void AdvanceToShop()
        {
            sceneLoader.LoadScene(shopIndex);
        }

        //========= Handle Destroy ==========
        private void OnDestroy()
        {
            EventBus<SceneLoadedEvent>.RemoveListener(HandleSceneLoaded);
            EventBus<ShopLoadedEvent>.RemoveListener(HandleShopLoad);
            //announce game end
            EventBus<GameEndEvent>.Invoke(new GameEndEvent());
        }
    }
}
