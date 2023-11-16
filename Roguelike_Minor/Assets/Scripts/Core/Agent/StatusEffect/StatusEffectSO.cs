using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core {
    public abstract class StatusEffectSO : ScriptableObject
    {
        public Sprite icon;

        //============== Manage Effect addition / subtraction ===============
        public abstract void AddEffect(StatusEffectHandler handler);
        public abstract void RemoveEffect(StatusEffectHandler handler);

        //============== Manage Effect stacking ==================
        public abstract void AddStacks(StatusEffectHandler handler);
        public virtual void AddVars(StatusEffectHandler handler, List<StatusEffectHandler.EffectVars> vars)
        {
            vars.Add(new StatusEffectHandler.EffectVars());
        }

        public abstract void RemoveStacks(StatusEffectHandler handler);
        public virtual void RemoveVars(StatusEffectHandler handler, List<StatusEffectHandler.EffectVars> vars)
        {
            vars.RemoveAt(0);
        }

        //============== Process Events ================
        public virtual void ProcessDealDamage(ref HitEvent hitEvent, List<StatusEffectHandler.EffectVars> vars) { }
        public virtual void ProcessTakeDamage(ref HitEvent hitEvent, List<StatusEffectHandler.EffectVars> vars) { }
        public virtual void ProcessHealEvent(ref HealEvent healEvent, List<StatusEffectHandler.EffectVars> vars) { }
    }
}
