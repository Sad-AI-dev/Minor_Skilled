using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core {
    public class StatusEffectHandler : MonoBehaviour
    {
        [HideInInspector] public Agent agent;
        public Dictionary<StatusEffectSO, int> statusEffects = new();

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
            statusEffects.Add(effect, 0);
            effect.AddEffect(agent);
        }
        private void AddStacks(StatusEffectSO effect, int stacks)
        {
            effect.AddStacks(agent, stacks);
            statusEffects[effect] += stacks;
        }

        //==================== Manage Effect Removal ===================
        public void RemoveEffect(StatusEffectSO effect, int stacks = 1)
        {
            if (statusEffects.ContainsKey(effect))
            {
                RemoveStacks(effect, Mathf.Min(stacks, statusEffects[effect]));
                //remove effect check
                if (statusEffects[effect] == 0)
                {
                    RemoveEffect(effect);
                }
            }
        }

        private void RemoveStacks(StatusEffectSO effect, int stacksToRemove)
        {
            effect.RemoveStacks(agent, stacksToRemove);
            statusEffects[effect] -= stacksToRemove;
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
                RemoveEffect(kvp.Key, kvp.Value);
            }
            statusEffects.Clear();
        }

        //================ Process Heal / Hurt Event ==================
        public void ProcessHitEvent(ref HitEvent hitEvent)
        {
            foreach (var kvp in statusEffects)
            {
                kvp.Key.ProcessHitEvent(ref hitEvent, kvp.Value);
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
