using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game {
    public class ShopPurchasable : Purchasable
    {
        [SerializeField] private ItemPickup itemPickup;

        //========= Setup Item =========
        public void Setup(ItemDataSO item)
        {
            itemPickup.item = item;
            itemPickup.GenerateVisuals();
        }

        protected override bool CanPurchase(Agent agent)
        {
            return agent.inventory.TryAssignItem(itemPickup.item); //purchase item
        }
    }
}
