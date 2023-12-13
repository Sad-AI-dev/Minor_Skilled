using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game {
    [CreateAssetMenu(fileName = "0Slot_Piece", menuName = "ScriptableObjects/Items/T1/0: Slot Piece", order = 100)]
    public class Item0SO : ItemDataSO
    {
        [Header("Settings")]
        public int maxCount;

        [Header("Reward Settings")]
        public SlotSizeSO rewardSize;

        //========= Manage Stacks ===========
        public override void AddStack(Item item)
        {
            //complete slot check
            if (item.stacks >= maxCount) 
            {
                //reward slot
                (item.agent.inventory as SlotInventory).AddSlot(rewardSize);
                //remove items
                for (int i = 0; i < maxCount; i++) { item.agent.inventory.RemoveItem(this); }
            }
        }

        //========== Description ===========
        public override string GenerateLongDescription()
        {
            return $"If you are seeing this, something went very wrong.\n" +
                $"Kindly let Arvid know that his systems have shit the bed :)";
        }
    }
}
