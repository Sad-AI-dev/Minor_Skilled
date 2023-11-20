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
            public Coroutine activeRoutine;
        }

        public float attackSpeedIncrease = 0.1f;
        public float duration = 1f;

        //========= Manage Effect ==========
        public override void AddEffect(StatusEffectHandler handler) { }

        public override void RemoveEffect(StatusEffectHandler handler) { }

        //========== Manage Vars ==========
        public override void AddVars(StatusEffectHandler handler, List<StatusEffectHandler.EffectVars> vars)
        {
            vars.Add(new AttackSpeedEffectVars());
        }

        //========= Manage Stacks ==========
        public override void AddStack(StatusEffectHandler handler)
        {
            int varsIndex = handler.statusEffects[this].Count - 1;
            AttackSpeedEffectVars vars = handler.statusEffects[this][varsIndex] as AttackSpeedEffectVars;
            vars.activeRoutine = handler.agent.StartCoroutine(IncreaseAttackSpeedCo(handler.agent));
        }

        public override void RemoveStack(StatusEffectHandler handler)
        {
            handler.agent.stats.attackSpeed -= attackSpeedIncrease;
            RemoveStack(handler, handler.statusEffects[this][0] as AttackSpeedEffectVars);
        }
        private void RemoveStack(StatusEffectHandler handler, AttackSpeedEffectVars vars)
        {
            //stop coroutine
            if (vars.activeRoutine != null)
            {
                handler.agent.StopCoroutine(vars.activeRoutine);
                vars.activeRoutine = null;
            }
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
