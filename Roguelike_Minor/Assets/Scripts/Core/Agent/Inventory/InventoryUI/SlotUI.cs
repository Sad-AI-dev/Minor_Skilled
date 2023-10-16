using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.Core.Data;

namespace Game.Core {
    public class SlotUI : MonoBehaviour
    {
        public UnityDictionary<SlotSizeSO, List<ItemUI>> itemVisuals;

        public void GenerateVisuals(InventoryUI inventoryUI, Item[] items, int slotsToShow)
        {
            ResetVisuals();
            //generate item visuals
            for (int i = 0; i < slotsToShow; i++)
            {
                //configure item UI
                ItemUI targetUI = itemVisuals[GetTargetSize(items[i])][i];
                targetUI.inventoryUI = inventoryUI; //pass inventoryUI reference
                targetUI.GenerateVisuals(items[i]);
            }
        }
        private SlotSizeSO GetTargetSize(Item item)
        {
            return item != null ? item.data.size : itemVisuals.Keys.First();
        }

        //======== Reset =========
        public void ResetVisuals()
        {
            //reset item visuals
            foreach (var kvp in itemVisuals)
            {
                foreach (ItemUI itemUI in kvp.Value)
                {
                    itemUI.ResetVisuals();
                }
            }
        }
    }
}
