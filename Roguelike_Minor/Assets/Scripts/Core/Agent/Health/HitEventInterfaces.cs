using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core {
    public interface IEventProcessor
    {
        public int GetPriority();

        public int CompareTo(IEventProcessor other)
        {
            if (GetPriority() > other.GetPriority()) { return -1; } //this priority is higher, move to earlier in list
            else if (GetPriority() == other.GetPriority()) { return 0; } //same priority, don't change anything
            else { return 1; } //this priority is lower, move to later in list
        }
    }

    public interface IDealDamageProcessor : IEventProcessor
    {
        public void ProcessDealDamage(ref HitEvent hitEvent, Item sourceItem);
        public void ProcessDealDamage(ref HitEvent hitevent, List<StatusEffectHandler.EffectVars> vars);
    }

    public interface ITakeDamageProcessor : IEventProcessor
    {
        public void ProcessTakeDamage(ref HitEvent hitEvent, Item sourceItem);
        public void ProcessTakeDamage(ref HitEvent hitEvent, List<StatusEffectHandler.EffectVars> vars);
    }

    public interface IHealProcessor : IEventProcessor
    {
        public void ProcessHeal(ref HealEvent healEvent, Item sourceItem);
        public void ProcessHeal(ref HealEvent healEvent, List<StatusEffectHandler.EffectVars> vars);
    }
}
