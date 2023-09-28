using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game {
    [CreateAssetMenu(fileName = "Effect", menuName = "ScriptableObjects/Agent/StatusEffect/TestEffect")]
    public class TestEffectSO : StatusEffectSO
    {
        public float attackSpeedBonus = 1f;

        //============ Manage Add / Remove ==============
        public override void AddEffect(Agent agent)
        {

        }

        public override void RemoveEffect(Agent agent)
        {

        }

        //============ Manage Stacks ================
        public override void AddStacks(Agent agent, int stacks = 1)
        {
            for (int i = 0; i < stacks; i++)
            {
                agent.stats.attackSpeed += attackSpeedBonus;
            }
        }

        public override void RemoveStacks(Agent agent, int stacks = 1)
        {
            for (int i = 0; i < stacks; i++)
            {
                agent.stats.attackSpeed -= attackSpeedBonus;
            }
        }

        //============== Process Hit / Heal Events ================
        public override void ProcessTakeDamage(ref HitEvent hitEvent, int stacks)
        {
            hitEvent.damageReduction += stacks;
        }
        public override void ProcessDealDamage(ref HitEvent hitEvent, int stacks = 1)
        {
            
        }
        public override void ProcessHealEvent(ref HealEvent healEvent, int stacks)
        {
            healEvent.healMultiplier += stacks;
        }
    }
}