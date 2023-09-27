using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core {
    public abstract class Inventory : MonoBehaviour
    {
        [HideInInspector] public Agent agent;
        public List<Item> items;

        //=========== Manage Items ===============
        public abstract bool TryAssignItem(Item item);
        public abstract void RemoveItem(Item item);

        //=========== Drop Item ===========
        public abstract void DropItem(Item item);

        //=========== Process Hit / Heal Events =============
        public void ProcessHitEvent(ref HitEvent hitEvent)
        {
            if (hitEvent.source.Equals(agent))
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
