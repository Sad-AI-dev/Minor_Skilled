using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core {
    [System.Serializable]
    public class Item
    {
        [HideInInspector] public Agent agent;
        public ItemData data;
        public int stacks;

        //ctor
        public Item(ItemData data, Inventory holder)
        {
            agent = holder.agent;
            this.data = data;
            stacks = 1;
        }

        //=========== Manage Stacks ============
        public void AddStack()
        {
            stacks++;
            data.AddStack(this);
        }

        public void RemoveStack()
        {
            stacks--;
            data.RemoveStack(this);
        }
    }
}
