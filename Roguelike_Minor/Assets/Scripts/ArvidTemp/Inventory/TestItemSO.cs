using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game {
    [CreateAssetMenu(fileName = "item", menuName = "ScriptableObjects/Items/testItem")]
    public class TestItemSO : ItemDataSO
    {
        public float attackSpeedPerStack = 0.2f;

        //========== Manage Stacks ============
        public override void AddStack(Item item)
        {
            item.agent.stats.attackSpeed += attackSpeedPerStack;
        }

        public override void RemoveStack(Item item)
        {
            item.agent.stats.attackSpeed -= attackSpeedPerStack;
        }

        //========== Process Hit Events ===============
        public override void ProcessDealDamage(ref HitEvent hitEvent)
        {

        }

        public override void ProcessTakeDamage(ref HitEvent hitEvent)
        {
            Debug.Log("Took Damage!");
        }

        //============ Process Heal Events ================
        public override void ProcessHealEvent(ref HealEvent healEvent)
        {
            Debug.Log("Healing!");
        }
    }
}
