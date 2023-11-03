using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game {
    [CreateAssetMenu(fileName = "Attack_Speed_Effect", menuName = "ScriptableObjects/Status Effects/Attackspeed")]
    public class AttackSpeedEffectSO : StatusEffectSO
    {
        private class AttackSpeedEffectVars : StatusEffectHandler.EffectVars
        {
            public List<Coroutine> activeRoutines;
        }

        public float attackSpeedIncrease = 0.1f;
        public float duration = 1f;

        //========= Manage Effect ==========
        public override void AddEffect(StatusEffectHandler handler)
        {
            handler.statusEffects[this] = new AttackSpeedEffectVars() { 
                activeRoutines = new List<Coroutine>()
            };
        }

        public override void RemoveEffect(StatusEffectHandler handler) { }

        //========= Manage Stacks ==========
        public override void AddStacks(StatusEffectHandler handler, int stacks)
        {
            AttackSpeedEffectVars vars = handler.statusEffects[this] as AttackSpeedEffectVars;
            vars.activeRoutines.Add(handler.agent.StartCoroutine(IncreaseAttackSpeedCo(handler.agent)));
        }

        public override void RemoveStacks(StatusEffectHandler handler, int stacks)
        {
            handler.agent.stats.attackSpeed -= attackSpeedIncrease;
            (handler.statusEffects[this] as AttackSpeedEffectVars).activeRoutines.RemoveAt(0);
        }

        //======== Co Routine ==========
        private IEnumerator IncreaseAttackSpeedCo(Agent agent)
        {
            agent.stats.attackSpeed += attackSpeedIncrease;
            yield return new WaitForSeconds(duration);
            agent.effectHandler.RemoveEffect(this);
        }
    }
}
