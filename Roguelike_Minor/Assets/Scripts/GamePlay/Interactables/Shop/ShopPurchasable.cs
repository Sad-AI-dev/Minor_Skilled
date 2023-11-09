using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Core.Data;

namespace Game {
    public class ShopPurchasable : Purchasable
    {
        [SerializeField] private ItemPickup itemPickup;
        [SerializeField] private UnityDictionary<ItemTierSO, float> itemTierPriceMults;

        //========= Setup Item =========
        public void Setup(ItemDataSO item)
        {
            itemPickup.item = item;
            itemPickup.GenerateVisuals();
        }

        //========= Price Calculation ===========
        protected override int CalcPrice(float priceMult)
        {
            if (itemPickup.item)
            {
                return Mathf.RoundToInt(price * priceMult * itemTierPriceMults[itemPickup.item.tier]);
            }
            else { return base.CalcPrice(priceMult); }
        }

        //======= Purchase Check =========
        protected override bool CanPurchase(Agent agent)
        {
            return agent.inventory.TryAssignItem(itemPickup.item); //purchase item
        }
    }
}
