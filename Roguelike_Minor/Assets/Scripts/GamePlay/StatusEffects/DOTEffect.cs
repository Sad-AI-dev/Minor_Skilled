using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game {
    [CreateAssetMenu(fileName = "DOT Effect", menuName = "ScriptableObjects/Status Effects/DOT Effect")]
    public class DOTEffect : StatusEffectSO
    {
        public class DOTEffectVars : StatusEffectHandler.EffectVars
        {
            public Agent source;
            public Coroutine currentRoutine;
            public float dmg;
        }

        public float tickRate = 1f; //expressed in ticks / second
        public float duration = 5f;

        [Header("Visual Settings")]
        public Color numLabelColor = Color.white;

        public override void RemoveEffect(StatusEffectHandler handler) { }

        //========== Manage Effect =======
        public override void AddEffect(StatusEffectHandler handler) { }

        //========= Manage Vars =========
        public override void AddVars(StatusEffectHandler handler, List<StatusEffectHandler.EffectVars> vars)
        {
            handler.statusEffects[this].Add(new DOTEffectVars());
        }

        //========= Manage Stacks ===========
        public override void AddStack(StatusEffectHandler handler)
        {
            DOTEffectVars vars = handler.statusEffects[this][handler.statusEffects[this].Count - 1] as DOTEffectVars;
            vars.currentRoutine = handler.agent.StartCoroutine(DOTCo(vars, handler.agent));
        }

        public override void RemoveStack(StatusEffectHandler handler)
        {
            DOTEffectVars vars = handler.statusEffects[this][0] as DOTEffectVars;
            //end coroutine
            if (vars.currentRoutine != null)
            {
                handler.agent.StopCoroutine(vars.currentRoutine);
                vars.currentRoutine = null;
            }
        }

        //=========== Co Routine ========
        private IEnumerator DOTCo(DOTEffectVars vars, Agent target)
        {
            for (int i = 0; i < GetTotalTicks(); i++)
            {
                yield return new WaitForSeconds(1f / tickRate); //tick rate is in ticks / second
                //create hitEvent
                HitEvent hitEvent = new HitEvent(vars.source, 0f)
                {
                    baseDamage = vars.dmg,
                    labelColor = numLabelColor
                };
                target.health.Hurt(hitEvent);
            }
            //remove stack
            target.effectHandler.RemoveEffect(this);
        }

        //=========== Util ===========
        public float GetTotalTicks()
        {
            return tickRate * duration;
        }
    }
}
