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
        public StatTracker statTracker; //required by resultscreen

        [Header("Planet Advancement Settings")]
        [SerializeField] private List<ListWrapper<int>> planetSceneIndeces;
        [SerializeField] private int shopIndex;
        [SerializeField] private int minShopDelay = 0;
        [SerializeField] private int maxShopDelay = 3;

        //ref to advance object spawner
        [HideInInspector] public AdvanceObjectSpawner advanceObjectSpawner;
        [HideInInspector] public LootSpawner lootSpawner; //needed for some items

        //scene advancement vars
        public int currentStage { get; private set; } = 1;
        private int stagesTillShop;

        //paused state
        [HideInInspector] public bool scalingIsPaused;
        private bool isShopStage;

        private void Setup()
        {
            //setup events
            EventBus<SceneLoadedEvent>.AddListener(HandleSceneLoaded);
            EventBus<ShopLoadedEvent>.AddListener(HandleShopLoad);
            //initialize stages till shop
            stagesTillShop = Random.Range(minShopDelay, maxShopDelay);
        }

        //========= Handle Scene Load ========
        private void HandleSceneLoaded(SceneLoadedEvent data)
        {
            //handle scaling pause
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
            //advance stage
            currentStage++;
            sceneLoader.LoadScene(GetNextStageBuildIndex());
        }
        private int GetNextStageBuildIndex()
        {
            int planetIndex = (currentStage - 1) % planetSceneIndeces.Count;
            return planetSceneIndeces[planetIndex][Random.Range(0, planetSceneIndeces[planetIndex].Count)];
        }

        public void AdvanceToNextStage()
        {
            if (stagesTillShop == 0)
            {
                AdvanceToShop();
            }
            else
            {
                AdvanceToNextPlanet();
                stagesTillShop--;
            }
        }
        private void AdvanceToShop()
        {
            sceneLoader.LoadScene(shopIndex);
            //reset stages till shop
            stagesTillShop = Random.Range(minShopDelay, maxShopDelay);
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
