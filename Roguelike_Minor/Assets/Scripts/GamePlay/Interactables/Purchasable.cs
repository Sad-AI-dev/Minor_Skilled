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
        [SerializeField] protected int basePrice = 20;
        protected int price;

        [Header("Technical Settings")]
        [SerializeField] private TMP_Text priceLabel;
        public UnityEvent<Agent> onPurchase;

        private void Start()
        {
            Initialize();
            
        }
        //========== Initialize =============
        public void Initialize()
        {
            //calc base price
            price = CalcPrice(GameScalingManager.instance.priceMult);
            //update UI
            if (priceLabel) { priceLabel.text = "$" + price; }
        }

        //========= Handle Price Increase =========
        protected virtual int CalcPrice(float priceMult)
        {
            return Mathf.RoundToInt(basePrice * priceMult);
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
