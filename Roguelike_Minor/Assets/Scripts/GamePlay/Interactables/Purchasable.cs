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
        [SerializeField] protected int price = 5;

        [Header("Technical Settings")]
        [SerializeField] private TMP_Text priceLabel;
        public UnityEvent<Agent> onPurchase;

        private void Start()
        {
            Initialize();
            if (priceLabel) { priceLabel.text = "$" + price; }
        }
        //========== Initialize =============
        private void Initialize()
        {
            //calc base price
            price = CalcPrice(GameScalingManager.instance.priceMult);
        }

        //========= Handle Price Increase =========
        protected virtual int CalcPrice(float priceMult)
        {
            return Mathf.RoundToInt(price * priceMult);
        }

        //========== Try Purchase =============
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
