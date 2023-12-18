using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using System.Linq;

namespace Game {
    [CreateAssetMenu(fileName = "Vulnerable", menuName = "ScriptableObjects/Status Effects/Vulnerable Effect")]
    public class VulnerableEffectSO : StatusEffectSO, ITakeDamageProcessor
    {
        public class VulnerableVars : StatusEffectHandler.EffectVars
        {
            public float damageMult;
            public ItemDataSO source;

            public override void Copy(StatusEffectHandler.EffectVars otherVars)
            {
                VulnerableVars vars = otherVars as VulnerableVars;
                damageMult = vars.damageMult;
            }
        }

        public int priority;
        public float duration;
        public Color labelColor;

        public int GetPriority() { return priority; }

        //======== Manage Effect =============
        public override void AddEffect(StatusEffectHandler handler) { }
        public override void RemoveEffect(StatusEffectHandler handler) { }

        //======== Manage Vars =============
        public override void AddVars(StatusEffectHandler handler, List<StatusEffectHandler.EffectVars> vars)
        {
            vars.Add(new VulnerableVars());
        }

        //======== Manage Stacks ===========
        public override void AddStack(StatusEffectHandler handler) 
        {
            handler.StartCoroutine(DurationCo(handler));
        }
        public override void RemoveStack(StatusEffectHandler handler) { }

        //==== Duration Routine ===
        private IEnumerator DurationCo(StatusEffectHandler handler)
        {
            yield return new WaitForSeconds(duration);
            handler.RemoveEffect(this);
        }

        //========== Manage Take Damage ==============
        public void ProcessTakeDamage(ref HitEvent hitEvent, List<StatusEffectHandler.EffectVars> vars)
        {
            float totalDamageMult = 0f;
            foreach (VulnerableVars vulnerableVars in vars.Cast<VulnerableVars>())
            {
                totalDamageMult += vulnerableVars.damageMult;
            }
            hitEvent.damageMultiplier += totalDamageMult;
        }
        public void ProcessTakeDamage(ref HitEvent hitEvent, Item item) { }
    }
}
