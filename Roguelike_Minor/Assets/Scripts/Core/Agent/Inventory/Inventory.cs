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

        //==== Process take damage ====
        public void ProcessTakeDamage(ref HitEvent hitEvent, ITakeDamageProcessor processor)
        {
            processor.ProcessTakeDamage(ref hitEvent, GetItemOfType(processor as ItemDataSO));
        }

        //==== Process deal damage ====
        public void ProcessDealDamage(ref HitEvent hitEvent, IDealDamageProcessor processor)
        {
            Item item = GetItemOfType(processor as ItemDataSO);
            if (CanTriggerItem(ref hitEvent, item))
            {
                processor.ProcessDealDamage(ref hitEvent, item);
            }
        }
        private bool CanTriggerItem(ref HitEvent hitEvent, Item item)
        {
            return !hitEvent.itemSources.Contains(item) || item.data.canProcSelf;
        }

        //==== Process heal ====
        public void ProcessHealEvent(ref HealEvent healEvent, IHealProcessor processor)
        {
            processor.ProcessHeal(ref healEvent, GetItemOfType(processor as ItemDataSO));
        }
    }
}
