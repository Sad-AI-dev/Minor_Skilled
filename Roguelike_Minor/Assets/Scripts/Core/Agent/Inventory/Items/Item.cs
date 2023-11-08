using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core {
    [System.Serializable]
    public class Item
    {
        public class ItemVars { }

        [HideInInspector] public Agent agent;
        public ItemDataSO data;
        public int stacks;
        //vars
        public ItemVars vars;

        //ctor
        public Item(ItemDataSO data, Inventory holder)
        {
            agent = holder.agent;
            this.data = data;
            this.data.InitializeVars(this);
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
            GameObject obj = Object.Instantiate(data.pickupPrefab);
            //setup item vars
            obj.GetComponent<ItemPickup>().item = data;
            //set item pos
            obj.transform.position = agent.transform.position;
        }
    }
}
