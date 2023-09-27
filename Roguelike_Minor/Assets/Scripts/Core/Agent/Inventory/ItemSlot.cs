using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core {
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

        public void DropItem(Item item)
        {
            //TODO implement drop item
        }
    }
}
