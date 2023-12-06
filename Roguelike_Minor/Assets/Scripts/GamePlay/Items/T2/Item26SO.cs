using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game {
    [CreateAssetMenu(fileName = "26Haggler's_Delight", menuName = "ScriptableObjects/Items/T2/26: Haggler's Delight", order = 226)]
    public class Item26SO : ItemDataSO
    {
        [Header("Rerolls settings")]
        public int baseRerolls = 1;
        public int bonusRerolls = 1;

        //static var
        private static int rerolls = 0;

        //========= Manage Stacks ===========
        public override void AddStack(Item item)
        {
            if (item.stacks == 1) { 
                rerolls = baseRerolls;
                EventBus<ShopLoadedEvent>.AddListener(OnShopLoad);
                EventBus<GameEndEvent>.AddListener(OnGameEnd);
            }
            else { rerolls += bonusRerolls; }
        }

        public override void RemoveStack(Item item)
        {
            if (item.stacks == 0) { 
                rerolls = 0;
                EventBus<ShopLoadedEvent>.RemoveListener(OnShopLoad);
                EventBus<GameEndEvent>.RemoveListener(OnGameEnd);
            }
            else { rerolls -= bonusRerolls; }
        }

        //========== Handle Events ============
        private void OnShopLoad(ShopLoadedEvent eventData)
        {
            eventData.shop.rerolls += rerolls;
        }

        private void OnGameEnd(GameEndEvent eventData)
        {
            EventBus<ShopLoadedEvent>.RemoveListener(OnShopLoad);
            EventBus<GameEndEvent>.RemoveListener(OnGameEnd);
        }

        //========== Description ===========
        public override string GenerateLongDescription()
        {
            return $"Gain the ability to <color=#{HighlightColor}>reroll items</color> " +
                $"in the <color=#{HighlightColor}>shop</color> " +
                $"<color=#{HighlightColor}>{baseRerolls}</color> " +
                $"<color=#{StackColor}>(+{bonusRerolls} per stack)</color> times";
        }
    }
}
