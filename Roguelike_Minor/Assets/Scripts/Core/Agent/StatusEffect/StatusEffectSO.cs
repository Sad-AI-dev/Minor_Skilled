using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core {
    public abstract class StatusEffectSO : ScriptableObject
    {
        //============== Manage Effect addition / subtraction ===============
        public abstract void AddEffect(StatusEffectHandler handler);
        public abstract void RemoveEffect(StatusEffectHandler handler);

        //============== Manage Effect stacking ==================
        public abstract void AddStacks(StatusEffectHandler handler, int stacks = 1);
        public abstract void RemoveStacks(StatusEffectHandler handler, int stacks = 1);

        //============== Process Events ================
        public virtual void ProcessDealDamage(ref HitEvent hitEvent, StatusEffectHandler.EffectVars vars) { }
        public virtual void ProcessTakeDamage(ref HitEvent hitEvent, StatusEffectHandler.EffectVars vars) { }
        public virtual void ProcessHealEvent(ref HealEvent healEvent, StatusEffectHandler.EffectVars vars) { }
    }
}
