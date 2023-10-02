using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core.Data;

namespace Game.Core {
    public class SlotUI : MonoBehaviour
    {
        public UnityDictionary<SlotSizeSO, List<ItemUI>> itemVisuals;

        public void GenerateVisuals(ItemSlot slot, InventoryUI inventoryUI)
        {
            ResetVisuals();
            //generate item visuals
            for (int i = 0; i < slot.heldItems.Count; i++)
            {
                ItemUI targetUI = itemVisuals[slot.heldItems[i].data.size][i];
                targetUI.inventoryUI = inventoryUI; //pass inventoryUI reference
                targetUI.GenerateVisuals(slot.heldItems[i]);
            }
        }

        //======== Reset =========
        private void ResetVisuals()
        {
            foreach (var kvp in itemVisuals)
            {
                foreach (ItemUI itemUI in kvp.Value)
                {
                    itemUI.img.color = new Color(1, 1, 1, 0); //hide element
                    itemUI.item = null;
                }
            }
        }
    }
}
