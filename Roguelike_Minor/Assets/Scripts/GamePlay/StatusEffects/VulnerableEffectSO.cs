using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using System.Linq;

namespace Game {
    [CreateAssetMenu(fileName = "Vulnerable", menuName = "ScriptableObjects/Status Effects/Vulnerable Effect")]
    public class VulnerableEffectSO : StatusEffectSO
    {
        public class VulnerableVars : StatusEffectHandler.EffectVars
        {
            public float damageMult;
        }

        public float duration;
        public Color labelColor;

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
        public override void ProcessTakeDamage(ref HitEvent hitEvent, List<StatusEffectHandler.EffectVars> vars)
        {
            float totalDamageMult = 0f;
            foreach (VulnerableVars vulnerableVars in vars.Cast<VulnerableVars>())
            {
                totalDamageMult += vulnerableVars.damageMult;
            }
            hitEvent.damageMultiplier += totalDamageMult;
        }
    }
}
