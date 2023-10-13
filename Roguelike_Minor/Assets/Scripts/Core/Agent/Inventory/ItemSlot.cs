using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core {
    [System.Serializable]
    public class ItemSlot
    {
        private const int maxSize = 4;
        public int size;
        public List<Item> heldItems;

        private int capacity;

        public ItemSlot(SlotSizeSO size)
        {
            this.size = size.capacity;
            capacity = size.capacity;
            heldItems = new List<Item>();
        }

        //============= Space Check ===========
        public bool HasSpace(SlotSizeSO size)
        {
            return capacity >= size.capacity;
        }
        public bool HasItemOfType(ItemDataSO data)
        {
            for (int i = 0; i < heldItems.Count; i++)
            {
                if (heldItems[i].data.Equals(data))
                {
                    return true;
                }
            }
            return false;
        }

        //============= Manage Items =============
        public void AssignItem(Item item)
        {
            heldItems.Add(item);
            capacity -= item.data.size.capacity;
        }

        public void RemoveItem(Item item)
        {
            heldItems.Remove(item);
            capacity += item.data.size.capacity;
        }

        //============ Manage Size ===========
        public bool CanAddSize(int sizeToAdd)
        {
            return size + sizeToAdd <= maxSize;
        }

        public void AddSize(int size)
        {
            this.size += size;
            capacity += size;
        }

        public bool CanRemoveSize(int sizeToRemove)
        {
            return size >= sizeToRemove;
        }

        public List<Item> RemoveSize(int size)
        {
            List<Item> itemsToRemove = new List<Item>();
            this.size -= size;
            capacity -= size;
            //remove items that no longer fit in slot
            if (capacity < 0)
            {
                int itemSizeToRemove = capacity * -1;
                while (itemSizeToRemove > 0)
                {
                    itemSizeToRemove -= heldItems[^1].data.size.capacity;
                    itemsToRemove.Add(heldItems[^1]);
                }
            }
            return itemsToRemove;
        }

        //============ Util ===========
        private int GetTotalItemSize()
        {
            int totalSize = 0;
            foreach (Item item in heldItems)
            {
                totalSize += item.data.size.capacity;
            }
            return totalSize;
        }
    }
}
