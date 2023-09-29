using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core.Data;

namespace Game.Core {
    public class InventoryUI : MonoBehaviour
    {
        public SlotInventory inventory;
        public BehaviourPool<SlotUI> smallItemVisuals;
        public BehaviourPool<SlotUI> mediumItemViuals;
        private ItemUI hoveredItem;

        //========= Create Visuals ===========
        public void GenerateVisuals()
        {

        }

        //============== Drop Item ==============
        public void TryDropItem()
        {
            if (hoveredItem != null)
            {
                inventory.DropItem(hoveredItem.item);
            }
            //reset visuals
            GenerateVisuals();
        }

        //============== Set Hover Item =============
        public void SetHoveredItem(ItemUI hoveredItem)
        {
            this.hoveredItem = hoveredItem;
        }
    }
}
