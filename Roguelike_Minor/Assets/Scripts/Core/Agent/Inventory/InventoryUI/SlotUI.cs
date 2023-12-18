using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core.Data;

namespace Game.Core {
    public class SlotUI : MonoBehaviour
    {
        public UnityDictionary<SlotSizeSO, List<ItemUI>> itemVisuals;

        public void GenerateVisuals(InventoryUI inventoryUI, Item[] items, int slotsToShow)
        {
            ResetVisuals();
            //last item
            int lastItemIndex = -1;
            //generate item visuals
            for (int i = 0; i < slotsToShow; i++)
            {
                if (items[i] != null) { lastItemIndex = i; }
                //configure item UI
                ItemUI targetUI = itemVisuals[GetTargetSize(items[i])][i];
                targetUI.inventoryUI = inventoryUI; //pass inventoryUI reference
                targetUI.GenerateVisuals(items[i], ShouldShowBG(items, i, lastItemIndex));
            }
        }
        private SlotSizeSO GetTargetSize(Item item)
        {
            return item != null ? item.data.size : itemVisuals.Keys.First();
        }

        private bool ShouldShowBG(Item[] items, int index, int lastItemIndex)
        {
            if (items[index] == null)
            {
                //check if previous item exists
                if (lastItemIndex < 0) { return true; } //no previous item, always show
                //check if in range of last item
                if (index < items[lastItemIndex].data.size.capacity + lastItemIndex) { return false; } //within bounds of last item
            }
            return true; //show by default
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
