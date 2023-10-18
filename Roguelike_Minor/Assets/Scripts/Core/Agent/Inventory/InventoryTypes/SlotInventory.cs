using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core {
    public class SlotInventory : Inventory
    {
        public int slots;

        //vars
        private int totalItemSize = 0;

        //============ Add Item ===============
        public override bool TryAssignItem(ItemDataSO itemData)
        {
            if (slots - totalItemSize >= itemData.size.capacity)
            {
                AddItemStack(itemData);
                SortItems();
                onContentsChanged?.Invoke();
                return true;
            }
            return false;
        }

        private void AddItemStack(ItemDataSO itemData)
        {
            //add stack to duplicate
            int dupIndex = GetItemIndex(itemData);
            if (dupIndex >= 0)
            {
                items[dupIndex].AddStack();
            }
            else //add new item
            {
                Item item = new Item(itemData, this);
                items.Add(item);
            }
            //increase total item size
            totalItemSize += itemData.size.capacity;
        }

        //============ Remove Item =================
        public override void RemoveItem(ItemDataSO itemData)
        {
            int itemIndex = GetItemIndex(itemData);
            if (itemIndex >= 0)
            {
                totalItemSize -= itemData.size.capacity;
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

        //============ Drop Item ================
        public override void DropItem(Item item)
        {
            RemoveItem(item.data);
            item.DropItem();
        }

        //============= Slot Management ============
        public void AddSlot(SlotSizeSO size)
        {
            slots += size.capacity;
            onContentsChanged?.Invoke();
        }

        public void RemoveSlot(SlotSizeSO size)
        {
            //drop items if needed
            int itemsToRemove = (-slots + totalItemSize) + size.capacity;
            while (itemsToRemove > 0)
            {
                //remove items
                itemsToRemove -= items[^1].data.size.capacity;
                DropItem(items[^1]);
            }
            //remove capacity
            slots -= size.capacity;
            //update UI
            onContentsChanged?.Invoke();
        }

        //============ Sort Contents ===========
        private void SortItems()
        {
            items.Sort((Item a, Item b) => -a.data.size.capacity.CompareTo(b.data.size.capacity));
        }
    }
}
