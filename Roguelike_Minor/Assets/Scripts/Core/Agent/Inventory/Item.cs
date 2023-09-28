using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core {
    [System.Serializable]
    public class Item
    {
        [HideInInspector] public Agent agent;
        public ItemDataSO data;
        public int stacks;

        //ctor
        public Item(ItemDataSO data, Inventory holder)
        {
            agent = holder.agent;
            this.data = data;
            //initialize first stack
            AddStack();
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

        public void DropItem()
        {
            //TEMP solution, iterate later based on pickup system

            GameObject obj = Object.Instantiate(data.pickupPrefab);
            obj.transform.position = agent.transform.position;
        }
    }
}
