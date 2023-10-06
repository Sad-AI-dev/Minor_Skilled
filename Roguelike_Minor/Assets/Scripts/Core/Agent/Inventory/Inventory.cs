using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core {
    public abstract class Inventory : MonoBehaviour
    {
        [HideInInspector] public Agent agent;
        public List<Item> items;
        public Action onContentsChanged;

        //=========== Manage Items ===============
        //adds a single item stack to inventory
        public abstract bool TryAssignItem(ItemDataSO itemData);
        //removes a single item stack from inventory
        public abstract void RemoveItem(ItemDataSO itemData);

        //=========== Drop Item ===========
        //drops a single item stack from inventory
        public abstract void DropItem(Item item);

        //=============== Util Funcs ==============
        protected int GetItemIndex(ItemDataSO itemData)
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

        public Item GetItemOfType(ItemDataSO type)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].data.Equals(type))
                {
                    return items[i];
                }
            }
            return null;
        }

        //=========== Process Hit / Heal Events =============
        public void ProcessHitEvent(ref HitEvent hitEvent)
        {
            if (hitEvent.hasAgentSource && hitEvent.source.Equals(agent))
            {
                for (int i = 0; i < items.Count; i++)
                {
                    items[i].data.ProcessDealDamage(ref hitEvent);
                }
            }
            else
            {
                for (int i = 0; i < items.Count; i++)
                {
                    items[i].data.ProcessTakeDamage(ref hitEvent);
                }
            }
        }

        public void ProcessHealEvent(ref HealEvent healEvent)
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].data.ProcessHealEvent(ref healEvent);
            }
        }
    }
}
