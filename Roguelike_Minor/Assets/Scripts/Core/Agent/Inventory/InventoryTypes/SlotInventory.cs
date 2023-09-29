using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core {
    public class SlotInventory : Inventory
    {
        public List<ItemSlot> slots;

        //============ Add Item ===============
        public override bool TryAssignItem(ItemDataSO itemData)
        {
            ItemSlot targetSlot = GetSlotWithSpace(itemData.size);
            if (targetSlot != null)
            {
                AddItemStack(itemData, targetSlot);
                onContentsChanged?.Invoke();
                return true;
            }
            return false;
        }

        private ItemSlot GetSlotWithSpace(SlotSizeSO size)
        {
            foreach (ItemSlot slot in slots)
            {
                if (slot.HasSpace(size))
                {
                    return slot;
                }
            }
            return null;
        }

        private void AddItemStack(ItemDataSO itemData, ItemSlot targetSlot)
        {
            //add stack to duplicate
            int dupIndex = FindDuplicateIndex(itemData);
            if (dupIndex >= 0)
            {
                items[dupIndex].AddStack();
                targetSlot.AssignItem(items[dupIndex]);
            }
            else //add new item
            {
                Item item = new Item(itemData, this);
                items.Add(item);
                targetSlot.AssignItem(item);
            }
        }
        private int FindDuplicateIndex(ItemDataSO itemData)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].data.Equals(itemData))
                {
                    return i;
                }
            }
            return -1;
        }

        //============ Remove Item =================
        public override void RemoveItem(ItemDataSO itemData)
        {
            int itemIndex = GetItemIndex(itemData);
            if (itemIndex >= 0)
            {
                RemoveItemFromSlot(items[itemIndex]);
                RemoveItemStack(items[itemIndex]);
                onContentsChanged?.Invoke();
            }
        }
        private void RemoveItemStack(Item item)
        {
            int itemIndex = items.IndexOf(item);
            //remove stack
            items[itemIndex].RemoveStack();
            //remove from list check
            if (items[itemIndex].stacks <= 0)
            {
                items.Remove(item);
            }
        }
        private void RemoveItemFromSlot(Item item)
        {
            for (int i = slots.Count - 1; i >= 0; i--)
            {
                if (slots[i].HasItemOfType(item.data))
                {
                    slots[i].RemoveItem(item);
                    i = -1; //end loop
                }
            }
        }

        //============ Drop Item ================
        public override void DropItem(Item item)
        {
            RemoveItem(item.data);
            item.DropItem();
        }

        //============= Slot Management ============
        public void AddSlot(SlotSizeSO size)
        {
            slots.Add(new ItemSlot(size));
            SortSlots();
            onContentsChanged?.Invoke();
        }

        public void RemoveSlot(ItemSlot slot)
        {
            if (slots.Contains(slot))
            {
                //foreach (Item item in slot.heldItems)
                for (int i = slot.heldItems.Count - 1; i >= 0; i--)
                {
                    items[i].DropItem(); //drop expelled items
                    RemoveItemStack(items[i]);
                }
                slots.Remove(slot);
                slot = null; //destroy removed slot
                SortSlots();
                onContentsChanged?.Invoke();
            }
        }

        private void SortSlots()
        {
            slots.Sort((ItemSlot a, ItemSlot b) => a.size.capacity.CompareTo(b.size.capacity));
        }
    }
}
