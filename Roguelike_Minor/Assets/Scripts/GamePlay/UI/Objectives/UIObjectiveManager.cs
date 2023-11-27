using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Core.Data;
using Game.Core.GameSystems;

namespace Game {
    public class UIObjectiveManager : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private UIProgressBarHandler progressBar;

        [Header("Technical")]
        [SerializeField] private ObjectiveManager objectiveManager;
        [SerializeField] private BehaviourPool<UIObjectiveCard> cardPool;

        //vars
        private Dictionary<Objective, UIObjectiveCard> cards;

        private void Awake()
        {
            //setup events
            objectiveManager.onStateChanged += OnStateChanged;
            objectiveManager.onObjectiveCompleted += OnObjectiveCompleted;
            //initialize vars
            cards = new Dictionary<Objective, UIObjectiveCard>();
            //setup bus event
            EventBus<StageLoadedEvent>.AddListener(OnStageLoaded);
        }

        //========= Handle State Change ========
        private void OnStateChanged(Objective objective, ObjectiveStep step)
        {
            if (cards.ContainsKey(objective)) { UpdateCard(cards[objective], step); }
            else { CreateNewCard(objective, step); }
        }

        //======== Handle new Objective =============
        private void CreateNewCard(Objective objective, ObjectiveStep step)
        {
            UIObjectiveCard card = cardPool.GetBehaviour();
            card.transform.SetParent(transform); //make sure card is child of manager
            cards.Add(objective, card);
            //initialize card
            card.Initialize(objective, step, progressBar);
        }

        //======= Update Objective State ==========
        private void UpdateCard(UIObjectiveCard card, ObjectiveStep step)
        {
            card.OnStateChanged(step);
        }

        //========= Handle Objective Completion =========
        private void OnObjectiveCompleted(Objective objective)
        {
            if (cards.ContainsKey(objective))
            {
                cards[objective].OnObjectiveCompleted();
                cards[objective].gameObject.SetActive(false); //recycle card
                cards.Remove(objective);
            }
        }

        //========== Handle Scene Load ==========
        private void OnStageLoaded(StageLoadedEvent eventData) //actives after assigner, causing it to reset right after
        {
            //hard reset
            foreach (var kvp in cards)
            {
                OnObjectiveCompleted(kvp.Key);
            }
        }

        //====== Handle Destroy =======
        private void OnDestroy()
        {
            EventBus<StageLoadedEvent>.RemoveListener(OnStageLoaded);
        }
    }
}
