using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Game.Core.Data;
using Game.Core.GameSystems;

namespace Game {
    public class UIObjectiveCard : MonoBehaviour
    {
        [Header("Refs")]
        public TMP_Text title;
        public RectTransform stepCardHolder;

        [Header("Technical")]
        [SerializeField] private BehaviourPool<UIStepCard> stepCardPool;

        //vars
        [HideInInspector]
        public Dictionary<ObjectiveStep, UIStepCard> stepCards;

        private void Awake()
        {
            //initialize vars
            stepCards = new Dictionary<ObjectiveStep, UIStepCard>();
        }

        //====== Initialize =======
        public void Initialize(Objective objective, ObjectiveStep step)
        {
            //setup objective
            title.text = objective.data.displayName;
            //setup first card
            CreateCard(step);
            OnStateChanged(step);
        }

        private void CreateCard(ObjectiveStep step)
        {
            //create card object
            UIStepCard card = stepCardPool.GetBehaviour();
            card.transform.SetParent(stepCardHolder);
            //setup card data
            card.Setup(step);
            stepCards.Add(step, card);
        }

        //======= Handle State Change =========
        public void OnStateChanged(ObjectiveStep step)
        {
            if (stepCards.ContainsKey(step))
            {
                stepCards[step].HandleStateChange(step);
            }
        }

        //====== Handle Objective Completion ========
        public void OnObjectiveCompleted()
        {
            foreach (var kvp in stepCards)
            {
                kvp.Value.gameObject.SetActive(false); //recycle all cards
            }
            stepCards.Clear();
        }
    }
}
