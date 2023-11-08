using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Game.Core;
using Game.Core.GameSystems;

namespace Game {
    public class Purchasable : MonoBehaviour
    {
        [Header("Gameplay Settings")]
        [SerializeField] private int price = 5;

        [Header("Technical Settings")]
        [SerializeField] private TMP_Text priceLabel;
        public UnityEvent<Agent> onPurchase;

        private void Start()
        {
            if (priceLabel) { priceLabel.text = "$" + price; }
        }

        public void TryPurchase(Interactor interactor)
        {
            Agent agent = interactor.agent;
            if (agent && agent.stats.Money >= price)
            {
                if (CanPurchase(agent)) //additional prereqs
                {
                    agent.stats.Money -= price;
                    onPurchase?.Invoke(agent);
                }
            }
        }

        protected virtual bool CanPurchase(Agent agent)
        {
            return true;
        }
    }
}
