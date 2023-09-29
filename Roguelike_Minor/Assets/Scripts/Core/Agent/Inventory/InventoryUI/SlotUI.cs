using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core {
    public class SlotUI : MonoBehaviour
    {
        public List<ItemUI> itemVisuals;

        public void GenerateVisuals(ItemSlot slot, SlotInventory inventory)
        {
            ResetVisuals();
        }

        private void ResetVisuals()
        {
            foreach (ItemUI itemUI in itemVisuals)
            {
                itemUI.img.sprite = null;
            }
        }
    }
}
