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
        public abstract void AddStack(StatusEffectHandler handler);
        public virtual void AddVars(StatusEffectHandler handler, List<StatusEffectHandler.EffectVars> vars)
        {
            vars.Add(new StatusEffectHandler.EffectVars());
        }

        public abstract void RemoveStack(StatusEffectHandler handler);
        public virtual void RemoveVars(StatusEffectHandler handler, List<StatusEffectHandler.EffectVars> vars)
        {
            vars.RemoveAt(0);
        }
    }
}
