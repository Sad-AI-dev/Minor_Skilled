using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core {
    public class StatusEffectHandler : MonoBehaviour
    {
        public class EffectVars
        {
            public int stacks;
        }

        [HideInInspector] public Agent agent;
        public Dictionary<StatusEffectSO, EffectVars> statusEffects = new();

        //============== Manage Effect Adding =====================
        public void AddEffect(StatusEffectSO effect, int stacks = 1)
        {
            if (!statusEffects.ContainsKey(effect)) 
            {
                AddNewEffect(effect);
            }
            AddStacks(effect, stacks);
        }

        private void AddNewEffect(StatusEffectSO effect)
        {
            statusEffects.Add(effect, new EffectVars());
            effect.AddEffect(agent);
        }
        private void AddStacks(StatusEffectSO effect, int stacks)
        {
            effect.AddStacks(agent, stacks);
            statusEffects[effect].stacks += stacks;
        }

        //==================== Manage Effect Removal ===================
        public void RemoveEffect(StatusEffectSO effect, int stacks = 1)
        {
            if (statusEffects.ContainsKey(effect))
            {
                RemoveStacks(effect, Mathf.Min(stacks, statusEffects[effect].stacks));
                //remove effect check
                if (statusEffects[effect].stacks == 0)
                {
                    RemoveEffect(effect);
                }
            }
        }

        private void RemoveStacks(StatusEffectSO effect, int stacksToRemove)
        {
            effect.RemoveStacks(agent, stacksToRemove);
            statusEffects[effect].stacks -= stacksToRemove;
        }
        private void RemoveEffect(StatusEffectSO effect)
        {
            effect.RemoveEffect(agent);
            statusEffects.Remove(effect);
        }

        //========== Clear ==========
        public void Clear()
        {
            foreach (var kvp in statusEffects)
            {
                RemoveEffect(kvp.Key, kvp.Value.stacks);
            }
            statusEffects.Clear();
        }

        //================ Process Heal / Hurt Event ==================
        public void ProcessHitEvent(ref HitEvent hitEvent)
        {
            if (hitEvent.hasAgentSource && hitEvent.source.Equals(agent)) //is source of hitEvent
            {
                foreach (var kvp in statusEffects)
                {
                    kvp.Key.ProcessDealDamage(ref hitEvent, kvp.Value);
                }
            }
            else
            {
                foreach (var kvp in statusEffects)
                {
                    kvp.Key.ProcessTakeDamage(ref hitEvent, kvp.Value);
                }
            }
        }

        public void ProcessHealEvent(ref HealEvent healEvent)
        {
            foreach (var kvp in statusEffects)
            {
                kvp.Key.ProcessHealEvent(ref healEvent, kvp.Value);
            }
        }
    }
}
