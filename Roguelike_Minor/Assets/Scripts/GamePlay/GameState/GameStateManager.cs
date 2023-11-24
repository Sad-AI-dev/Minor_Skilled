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
                Setup();
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

        [Header("Planet Advancement Settings")]
        [SerializeField] private List<ListWrapper<int>> planetSceneIndeces;
        [SerializeField] private int shopIndex;

        //ref to advance object spawner
        [HideInInspector] public AdvanceObjectSpawner advanceObjectSpawner;
        [HideInInspector] public LootSpawner lootSpawner; //needed for some items

        //scene advancement vars
        public int CurrentStage { get; private set; } = 1;

        //paused state
        [HideInInspector] public bool scalingIsPaused;
        private bool isShopStage;

        private void Setup()
        {
            //setup events
            EventBus<SceneLoadedEvent>.AddListener(HandleSceneLoaded);
            EventBus<ShopLoadedEvent>.AddListener(HandleShopLoad);
            EventBus<ObjectiveCompleteEvent>.AddListener(HandleObjectiveComplete);
        }

        //========== Manage Stage State ==============
        private void HandleObjectiveComplete(ObjectiveCompleteEvent eventData)
        {
            if (eventData.objectiveManager.AllObjectivesCompleted())
            {
                //TEMP
                advanceObjectSpawner.SpawnAdvanceObject();
                //update UI manager
                uiManager.ObjectiveComplete = true;
            }
        }

        public void HandleCompleteStageObject()
        {
            advanceObjectSpawner.SpawnAdvanceObject();
            //update UI manager
            uiManager.ObjectiveComplete = true;
        }

        //========= Handle Scene Load ========
        private void HandleSceneLoaded(SceneLoadedEvent data)
        {
            uiManager.ObjectiveComplete = false;
            //handle pause
            scalingIsPaused = isShopStage;
            //notify stage was loaded
            if (!isShopStage) { EventBus<StageLoadedEvent>.Invoke(new StageLoadedEvent()); }
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
            EventBus<ObjectiveCompleteEvent>.RemoveListener(HandleObjectiveComplete);
            //announce game end
            EventBus<GameEndEvent>.Invoke(new GameEndEvent());
        }
    }
}
