using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core {
    public class SimpleInventory : Inventory
    {
        //================= Add Item ===============
        public override bool TryAssignItem(ItemDataSO itemData)
        {
            int itemIndex = GetIndexWithItemData(itemData);
            if (itemIndex >= 0) //inventory already contains item of this type
            {
                items[itemIndex].AddStack();
            }
            else //add new type to inventory
            {
                items.Add(new Item(itemData, this));
            }
            onContentsChanged?.Invoke();
            return true;
        }
        private int GetIndexWithItemData(ItemDataSO itemData)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].data.Equals(itemData))
                {
                    return i;
                }
            }
            return -1; //nothing found
        }

        //=================== Remove Item ================
        public override void RemoveItem(ItemDataSO itemData)
        {
            int itemIndex = GetItemIndex(itemData);
            if (itemIndex >= 0)
            {
                items[itemIndex].RemoveStack();
                if (items[itemIndex].stacks <= 0)
                {
                    items.Remove(items[itemIndex]);
                }
                onContentsChanged?.Invoke();
            }
        }

        //=============== Drop Item =================
        public override void DropItem(Item item)
        {
            RemoveItem(item.data);
            item.DropItem();
        }
    }
}
