using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.Core.Data;

namespace Game.Core {
    public class SlotUI : MonoBehaviour
    {
        public UnityDictionary<SlotSizeSO, List<ItemUI>> itemVisuals;
        public List<Image> bgImages;

        public void GenerateVisuals(ItemSlot slot, InventoryUI inventoryUI)
        {
            ResetVisuals();
            //generate gbImages
            for (int i = 0; i < slot.size; i++)
            {
                bgImages[i].enabled = true;
            }
            //generate item visuals
            for (int i = 0; i < slot.heldItems.Count; i++)
            {
                ItemUI targetUI = itemVisuals[slot.heldItems[i].data.size][i];
                targetUI.inventoryUI = inventoryUI; //pass inventoryUI reference
                targetUI.GenerateVisuals(slot.heldItems[i]);
            }
        }

        //======== Reset =========
        public void ResetVisuals()
        {
            //reset bg images
            foreach (Image img in bgImages)
            {
                img.enabled = false;
            }
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
