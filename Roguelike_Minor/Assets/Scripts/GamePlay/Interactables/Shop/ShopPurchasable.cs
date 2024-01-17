using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Core.Data;

namespace Game {
    public class ShopPurchasable : Purchasable
    {
        [Header("Visual Settings")]
        [SerializeField] private MeshRenderer display;
        [SerializeField] private UnityDictionary<ItemTierSO, Material> mats;

        [Header("Refs")]
        [SerializeField] private ItemPickup itemPickup;
        [SerializeField] private UnityDictionary<ItemTierSO, float> itemTierPriceMults;

        //========= Setup Item =========
        public void Setup(ItemDataSO item)
        {
            itemPickup.item = item;
            itemPickup.GenerateVisuals();
            if (display) { SetDisplayMaterial(item.tier); }
            Initialize(); //recalc price
        }

        //========= Price Calculation ===========
        protected override int CalcPrice(float priceMult)
        {
            if (itemPickup.item)
            {
                return Mathf.RoundToInt(basePrice * priceMult * itemTierPriceMults[itemPickup.item.tier]);
            }
            else { return base.CalcPrice(priceMult); }
        }

        //======= Purchase Check =========
        protected override bool CanPurchase(Agent agent)
        {
            bool result = agent.inventory.TryAssignItem(itemPickup.item); //purchase item
            //notify item was picked up
            if (result) { EventBus<PickupItemEvent>.Invoke(new PickupItemEvent() { item = itemPickup.item }); }
            return result;
        }

        //======== Display Material =========
        private void SetDisplayMaterial(ItemTierSO tier)
        {
            Material[] displayMats = display.materials;
            displayMats[2] = mats[tier]; 
            display.materials = displayMats;
        }
    }
}
