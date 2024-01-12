using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game {
    [CreateAssetMenu(fileName = "27Eye_Candy", menuName = "ScriptableObjects/Items/T1/27: Eye Candy", order = 127)]
    public class Item27SO : ItemDataSO
    {
        private class Item27Vars : Item.ItemVars
        {
            public float totalChance;
        }

        [Header("Rarity Settings")]
        public float baseChance = 0.1f;
        public float bonusChance = 0.1f;

        //static var
        private static float totalChance;

        //========= Initialize vars ========
        public override void InitializeVars(Item item)
        {
            item.vars = new Item27Vars();
        }

        //========= Manage Stacks ===========
        public override void AddStack(Item item)
        {
            if (item.stacks == 1)
            {
                EventBus<ShopLoadedEvent>.AddListener(OnShopLoad);
                EventBus<GameEndEvent>.AddListener(HandleGameEnd);
            }
            totalChance = CalcTotalChance(item.stacks);
        }

        public override void RemoveStack(Item item)
        {
            if (item.stacks == 0) 
            {
                EventBus<ShopLoadedEvent>.RemoveListener(OnShopLoad);
                EventBus<GameEndEvent>.AddListener(HandleGameEnd);
            }
            totalChance = CalcTotalChance(item.stacks);
        }

        //=== Util ===
        private float CalcTotalChance(int stacks)
        {
            float total = stacks > 0 ? baseChance : 0f;
            for (int i = 1; i < stacks; i++)
            {
                total += (1f - total) * bonusChance;
            }
            return total;
        }

        //======== Handle Shop Load ==========
        private void OnShopLoad(ShopLoadedEvent eventData)
        {
            eventData.shop.itemLuck = totalChance;
        }

        //======= Handle Game End ========
        private void HandleGameEnd(GameEndEvent eventData)
        {
            EventBus<GameEndEvent>.RemoveListener(HandleGameEnd);
            EventBus<ShopLoadedEvent>.RemoveListener(OnShopLoad);
        }

        //========== Description ===========
        public override string GenerateLongDescription()
        {
            return $"Increase chance of higher <color=#{HighlightColor}>Rarity</color> items " +
                $"appearing in the shop by " +
                $" <color=#{HighlightColor}>{baseChance * 100}%</color> " +
                $"<color=#{StackColor}>(+{bonusChance * 100}% per stack)</color>";
        }
    }
}
