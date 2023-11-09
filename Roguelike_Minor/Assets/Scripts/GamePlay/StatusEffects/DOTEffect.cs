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
            public List<Coroutine> currentRoutines;
            public float dmg;
        }

        public float tickRate = 1f; //expressed in ticks / second
        public float duration = 5f;

        [Header("Visual Settings")]
        public Color numLabelColor = Color.white;

        //========= Manage Effect ============
        public override void AddEffect(StatusEffectHandler handler)
        {
            handler.statusEffects[this] = new DOTEffectVars() { currentRoutines = new List<Coroutine>() };
        }

        public override void RemoveEffect(StatusEffectHandler handler) { }

        //========= Manage Stacks ===========
        public override void AddStacks(StatusEffectHandler handler, int stacks = 1)
        {
            DOTEffectVars vars = handler.statusEffects[this] as DOTEffectVars;
            for (int i = 0; i < stacks; i++)
            {
                vars.currentRoutines.Add(handler.agent.StartCoroutine(DOTCo(vars, handler.agent)));
            }
        }

        public override void RemoveStacks(StatusEffectHandler handler, int stacks = 1)
        {
            DOTEffectVars vars = handler.statusEffects[this] as DOTEffectVars;
            for (int i = 0; i < stacks; i++)
            {
                RemoveStack(handler, vars);
            }
        }
        private void RemoveStack(StatusEffectHandler handler, DOTEffectVars vars)
        {
            //end coroutine
            if (vars.currentRoutines.Count > 0 && vars.currentRoutines[0] != null)
            {
                handler.agent.StopCoroutine(vars.currentRoutines[0]);
            }
            vars.currentRoutines.RemoveAt(0);
        }

        //=========== Co Routine ========
        private IEnumerator DOTCo(DOTEffectVars vars, Agent target)
        {
            for (int i = 0; i < tickRate * duration; i++)
            {
                yield return new WaitForSeconds(1f / tickRate); //tick rate is in ticks / second
                //create hitEvent
                HitEvent hitEvent = new HitEvent(vars.source)
                {
                    baseDamage = vars.dmg,
                    procCoef = 0f,
                    labelColor = numLabelColor
                };
                target.health.Hurt(hitEvent);
            }
            //remove stack
            target.effectHandler.RemoveEffect(this);
        }
    }
}
