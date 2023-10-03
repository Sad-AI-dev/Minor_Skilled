using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core {
    public abstract class StatusEffectSO : ScriptableObject
    {
        //============== Manage Effect addition / subtraction ===============
        public abstract void AddEffect(Agent agent);
        public abstract void RemoveEffect(Agent agent);

        //============== Manage Effect stacking ==================
        public abstract void AddStacks(Agent agent, int stacks = 1);
        public abstract void RemoveStacks(Agent agent, int stacks = 1);

        //============== Process Events ================
        public abstract void ProcessDealDamage(ref HitEvent hitEvent, int stacks = 1);
        public abstract void ProcessTakeDamage(ref HitEvent hitEvent, int stacks = 1);
        public abstract void ProcessHealEvent(ref HealEvent healEvent, int stacks = 1);
    }
}
