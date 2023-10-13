using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core.Data;

namespace Game.Core {
    public class InventoryUI : MonoBehaviour
    {
        public SlotInventory inventory;

        [Header("Slot Visual Settings")]
        public BehaviourPool<SlotUI> slotPool;
        public RectTransform slotHolder;

        //hover item
        private ItemUI hoveredItem;

        private void Awake()
        {
            inventory.onContentsChanged += GenerateVisuals;
        }

        //============== TEMP =============
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                TryDropItem();
            }
        }

        //========= Create Visuals ===========
        private void GenerateVisuals()
        {
            ResetVisuals();
            GenerateSlotVisuals();
        }

        private void ResetVisuals()
        {
            slotPool.Reset();
        }

        private void GenerateSlotVisuals()
        {
            for (int i = 0; i < inventory.slots.Count; i++)
            {
                ItemSlot targetSlot = inventory.slots[i];
                SlotUI slotUI = slotPool.GetBehaviour();
                slotUI.transform.SetParent(slotHolder);
                slotUI.GenerateVisuals(targetSlot, this);
            }
        }

        //============== Drop Item ==============
        public void TryDropItem()
        {
            if (hoveredItem != null)
            {
                inventory.DropItem(hoveredItem.item);
                //redraw visuals
                GenerateVisuals();
                //reset hoveredItem
                hoveredItem = null;
            }
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
