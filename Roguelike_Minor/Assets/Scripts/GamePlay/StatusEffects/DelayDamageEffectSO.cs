using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game {
    [CreateAssetMenu(fileName = "Doom", menuName = "ScriptableObjects/Status Effects/Delayed Damage Effect")]
    public class DelayDamageEffectSO : StatusEffectSO
    {
        public class DelayDamageEffectVars : StatusEffectHandler.EffectVars
        {
            public HitEvent sourceEvent;
            public float damageMult;

            public override void Copy(StatusEffectHandler.EffectVars otherVars)
            {
                DelayDamageEffectVars vars = otherVars as DelayDamageEffectVars;
                sourceEvent = vars.sourceEvent;
                damageMult = vars.damageMult;
            }
        }

        public float damageDelay = 0.5f;
        public Color labelColor;

        //======== Manage Effect =============
        public override void AddEffect(StatusEffectHandler handler) { }
        public override void RemoveEffect(StatusEffectHandler handler) { }

        //======== Manage Vars =============
        public override void AddVars(StatusEffectHandler handler, List<StatusEffectHandler.EffectVars> vars)
        {
            vars.Add(new DelayDamageEffectVars());
        }

        //======== Manage Stacks ===========
        public override void AddStack(StatusEffectHandler handler)
        {
            handler.StartCoroutine(DamageCo(handler.agent, handler.statusEffects[this][^1] as DelayDamageEffectVars));
        }

        public override void RemoveStack(StatusEffectHandler handler) { }

        //========= Doom Routine ==========
        private IEnumerator DamageCo(Agent target, DelayDamageEffectVars vars)
        {
            yield return new WaitForSeconds(damageDelay);
            //initialize sourceEvent
            vars.sourceEvent.damageMultiplier = vars.damageMult;
            vars.sourceEvent.labelColor = labelColor;
            //deal damage
            target.health.Hurt(vars.sourceEvent);
            //remove effect
            target.effectHandler.RemoveEffect(this);
        }
    }
}
