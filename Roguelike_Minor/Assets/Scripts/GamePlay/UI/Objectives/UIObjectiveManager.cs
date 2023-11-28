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
            //initialize vars
            cards = new Dictionary<Objective, UIObjectiveCard>();
            //setup events
            objectiveManager.onStateChanged += OnStateChanged;
            objectiveManager.onObjectiveCompleted += OnObjectiveCompleted;
            //setup bus event
            EventBus<SceneLoadedEvent>.AddListener(OnSceneLoaded);
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
        private void OnSceneLoaded(SceneLoadedEvent eventData)
        {
            //hard reset
            List<Objective> cardsCopy = new List<Objective>(cards.Keys);
            for (int i = cardsCopy.Count - 1; i >= 0; i--)
            { //this calls a remove func, so loop through in reverse order
                OnObjectiveCompleted(cardsCopy[i]);
            }
        }

        //====== Handle Destroy =======
        private void OnDestroy()
        {
            EventBus<SceneLoadedEvent>.RemoveListener(OnSceneLoaded);
        }
    }
}
