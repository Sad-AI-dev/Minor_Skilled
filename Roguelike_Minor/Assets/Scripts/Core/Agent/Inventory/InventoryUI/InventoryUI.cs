using System;
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

        [Header("Technical Settings")]
        [SerializeField] private int slotSize = 4;

        //hover item
        private ItemUI hoveredItem;
        public Action<ItemUI> onHoverItem;

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
            //create slots
            int itemIndex = 0;
            int stackIndex = 0;
            for (int i = 1; i <= inventory.slots; i += slotSize)
            { //each slot UI can hold up to 4, if there is atleast 1 slot for a set of 4, create a slot
                //configure slot
                SlotUI slotUI = slotPool.GetBehaviour();
                slotUI.transform.SetParent(slotHolder);
                slotUI.GenerateVisuals(this, GetSlotItems(ref itemIndex, ref stackIndex), GetSlotCount(i));
            }
        }

        //===== get all items for a slot =====
        private Item[] GetSlotItems(ref int itemIndex, ref int stackIndex)
        {
            Item[] slotItems = new Item[slotSize];
            int index = 0;
            while (index < slotSize)
            {
                if (TryValidateIndex(ref itemIndex, ref stackIndex))
                {
                    slotItems[index] = inventory.items[itemIndex];
                    index += inventory.items[itemIndex].data.size.capacity;
                    stackIndex++;
                }
                else { break; } //no more items to loop over
            }
            return slotItems;
        }

        private bool TryValidateIndex(ref int itemIndex, ref int stackIndex)
        {
            if (!IsValidIndex(itemIndex, stackIndex)) 
            {
                if (itemIndex >= inventory.items.Count - 1) { return false; } //cannot validate index, already looped through all items
                else //stackIndex is not valid
                {
                    itemIndex++;
                    stackIndex = 0;
                }
            }
            return true; //default; index is already valid
        }
        private bool IsValidIndex(int itemIndex, int stackIndex)
        {
            return itemIndex < inventory.items.Count && stackIndex < inventory.items[itemIndex].stacks;
        }

        //==== get slot count =====
        private int GetSlotCount(int index)
        {
            return Mathf.Min(slotSize, inventory.slots - (index - 1));
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
                SetHoveredItem(null);
            }
        }

        //============== Set Hover Item =============
        public void SetHoveredItem(ItemUI hoveredItem)
        {
            this.hoveredItem = hoveredItem;
            onHoverItem?.Invoke(hoveredItem);
        }

        private void OnDisable()
        {
            SetHoveredItem(null);
        }

        //============= OnDestroy =============
        private void OnDestroy()
        {
            inventory.onContentsChanged -= GenerateVisuals;
        }
    }
}
