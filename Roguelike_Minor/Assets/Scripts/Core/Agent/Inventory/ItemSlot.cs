using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core {
    [System.Serializable]
    public class ItemSlot
    {
        public SlotSizeSO size;
        public List<Item> heldItems;

        private int capacity;

        public ItemSlot(SlotSizeSO size)
        {
            this.size = size;
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
    }
}
