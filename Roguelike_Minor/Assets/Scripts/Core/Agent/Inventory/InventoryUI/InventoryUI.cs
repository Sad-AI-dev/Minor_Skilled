using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core.Data;

namespace Game.Core {
    public class InventoryUI : MonoBehaviour
    {
        [System.Serializable]
        public class VisualData
        {
            public BehaviourPool<SlotUI> slotPool;
            public RectTransform slotHolder;
        }

        public SlotInventory inventory;
        public UnityDictionary<SlotSizeSO, VisualData> slotVisuals;
        private ItemUI hoveredItem;

        private void Awake()
        {
            inventory.onContentsChanged += GenerateVisuals;
        }

        //========= Create Visuals ===========
        private void GenerateVisuals()
        {
            ResetVisuals();
            GenerateSlotVisuals();
        }

        private void ResetVisuals()
        {
            foreach (var kvp in slotVisuals)
            {
                kvp.Value.slotPool.Reset();
            }
        }

        private void GenerateSlotVisuals()
        {
            for (int i = 0; i < inventory.slots.Count; i++)
            {
                ItemSlot targetSlot = inventory.slots[i];
                SlotUI slotUI = slotVisuals[targetSlot.size].slotPool.GetBehaviour();
                slotUI.transform.SetParent(slotVisuals[targetSlot.size].slotHolder);
                slotUI.GenerateVisuals(targetSlot, inventory);
            }
        }

        //============== Drop Item ==============
        public void TryDropItem()
        {
            if (hoveredItem != null)
            {
                inventory.DropItem(hoveredItem.item);
            }
            //redraw visuals
            GenerateVisuals();
        }

        //============== Set Hover Item =============
        public void SetHoveredItem(ItemUI hoveredItem)
        {
            this.hoveredItem = hoveredItem;
        }

        //============= OnDestroy =============
        private void OnDestroy()
        {
            inventory.onContentsChanged -= GenerateVisuals;
        }
    }
}
