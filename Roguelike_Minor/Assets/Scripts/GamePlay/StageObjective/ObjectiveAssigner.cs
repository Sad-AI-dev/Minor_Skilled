using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Core.Data;
using Game.Core.GameSystems;

namespace Game {
    [RequireComponent(typeof(ObjectiveManager))]
    public class ObjectiveAssigner : MonoBehaviour
    {
        private ObjectiveManager manager;

        [Header("Objective Settings")]
        [SerializeField] private WeightedChance<List<ObjectiveDataSO>> objectives;

        private void Awake()
        {
            //get external components
            manager = GetComponent<ObjectiveManager>();
            //setup events
            EventBus<StageLoadedEvent>.AddListener(HandleStageLoad);
        }

        //====== Handle Scene Load ========
        private void HandleStageLoad(StageLoadedEvent sceneLoaded)
        {
            manager.Clear();
            if (!GameStateManager.instance.scalingIsPaused)
            {
                //pick objective
                List<ObjectiveDataSO> category = objectives.GetRandomEntry();
                ObjectiveDataSO data = category[Random.Range(0, category.Count)];
                //assign objective
                manager.AddObjective(new Objective(data));
            }
        }

        //========= Handle Destroy ========
        private void OnDestroy()
        {
            EventBus<StageLoadedEvent>.RemoveListener(HandleStageLoad);
        }
    }
}
