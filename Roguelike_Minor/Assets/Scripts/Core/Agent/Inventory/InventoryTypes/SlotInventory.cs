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
                SortInventory();
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
                    SortInventory();
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
            if (!TryAddSizeToExistingSlot(size.capacity))
            {
                slots.Add(new ItemSlot(size));
            }
            SortSlots();
            onContentsChanged?.Invoke();
        }
        private bool TryAddSizeToExistingSlot(int sizeToAdd)
        {
            foreach (ItemSlot slot in slots)
            {
                if (slot.CanAddSize(sizeToAdd))
                {
                    slot.AddSize(sizeToAdd);
                    return true;
                }
            }
            return false;
        }

        public void RemoveSlot(SlotSizeSO size)
        {
            for (int i = slots.Count - 1; i >= 0; i--)
            {
                if (slots[i].CanRemoveSize(size.capacity))
                {
                    List<Item> itemsToRemove = slots[i].RemoveSize(size.capacity);
                    foreach (Item item in itemsToRemove)
                    {
                        item.DropItem();
                        RemoveItemStack(item);
                    }
                    //remove slot check
                    if (slots[i].size <= 0)
                    {
                        slots[i] = null;
                        slots.RemoveAt(i);
                    }
                    SortSlots();
                    SortInventory();
                    onContentsChanged?.Invoke();
                    return; //stop loop
                }
            }
        }

        //============ Sort Contents ===========
        private void SortSlots()
        {
            slots.Sort((ItemSlot a, ItemSlot b) => -a.size.CompareTo(b.size));
        }

        private void SortInventory()
        {
            List<Item> slotItems = new List<Item>();
            //empty slots
            for (int i = slots.Count - 1; i >= 0; i--)
            {
                for (int j = slots[i].heldItems.Count - 1; j >= 0; j--)
                {
                    slotItems.Add(slots[i].heldItems[j]);
                    slots[i].RemoveItem(slots[i].heldItems[j]);
                }
            }
            //sort items based on size
            slotItems.Sort((Item a, Item b) => -a.data.size.capacity.CompareTo(b.data.size.capacity));
            //re-fill slots
            while (slotItems.Count > 0)
            {
                GetSlotWithSpace(slotItems[0].data.size).AssignItem(slotItems[0]);
                slotItems.RemoveAt(0);
            }
        }
    }
}
