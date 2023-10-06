using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game {
    public class AttackSpeedItemSO : ItemDataSO
    {
        public float baseAttackSpeedIncrease = 1f;
        public float stackAttackSpeedIncrease = 0.5f;

        //========= Manage Stacks ===========
        public override void AddStack(Item item)
        {
            if (item.stacks == 0) { item.agent.stats.attackSpeed += baseAttackSpeedIncrease; }
            else { item.agent.stats.attackSpeed += stackAttackSpeedIncrease; }
        }

        public override void RemoveStack(Item item)
        {
            if (item.stacks == 1) { item.agent.stats.attackSpeed -= baseAttackSpeedIncrease; }
            else { item.agent.stats.attackSpeed -= stackAttackSpeedIncrease; }
        }

        //========= Process Hit Events ===========
        public override void ProcessDealDamage(ref HitEvent hitEvent) { }

        public override void ProcessTakeDamage(ref HitEvent hitEvent) { }

        //========= Process Heal Events ============
        public override void ProcessHealEvent(ref HealEvent healEvent) { }
    }
}
