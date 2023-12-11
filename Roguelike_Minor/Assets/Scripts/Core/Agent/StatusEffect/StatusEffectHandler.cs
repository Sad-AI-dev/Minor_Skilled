using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core {
    public class StatusEffectHandler : MonoBehaviour
    {
        public class EffectVars { }

        [HideInInspector] public Agent agent;
        public Dictionary<StatusEffectSO, List<EffectVars>> statusEffects = new();

        [Header("UI Settings")]
        public StatusEffectBar effectBar;

        //========= Initialize HUD elements ==========
        private void Start()
        {
            if (effectBar) { effectBar.handler = this; }
        }

        //============== Manage Effect Adding =====================
        public EffectVars AddEffect(StatusEffectSO effect)
        {
            AddEffect(effect, 1);
            return statusEffects[effect][^1];
        }
        public void AddEffect(StatusEffectSO effect, int stacks)
        {
            if (!statusEffects.ContainsKey(effect)) 
            {
                AddNewEffect(effect);
            }
            StartCoroutine(AddStacksCo(effect, stacks));
        }

        private void AddNewEffect(StatusEffectSO effect)
        {
            statusEffects.Add(effect, new List<EffectVars>());
            effect.AddEffect(this);
            if (effect is IEventProcessor) { agent.health.AddProcessor(effect as IEventProcessor); }
            if (effectBar) { effectBar.HandleAddEffect(effect); }
        }
        private IEnumerator AddStacksCo(StatusEffectSO effect, int stacks)
        {
            for (int i = 0; i < stacks; i++)
            {
                effect.AddVars(this, statusEffects[effect]);
                effect.AddStack(this);
                yield return null;
            }
            if (effectBar) { effectBar.HandleUpdateStacks(effect, statusEffects[effect].Count); }
        }

        //==================== Manage Effect Removal ===================
        public void RemoveEffect(StatusEffectSO effect, int stacks = 1)
        {
            if (statusEffects.ContainsKey(effect))
            {
                RemoveStacks(effect, Mathf.Min(stacks, statusEffects[effect].Count));
                //remove effect check
                if (statusEffects[effect].Count == 0)
                {
                    RemoveEffect(effect);
                }
            }
        }

        private void RemoveEffect(StatusEffectSO effect)
        {
            effect.RemoveEffect(this);
            statusEffects.Remove(effect);
            if (effect is IEventProcessor) { agent.health.RemoveProcessor(effect as IEventProcessor); }
            if (effectBar) { effectBar.HandleRemoveEffect(effect); }
        }
        private void RemoveStacks(StatusEffectSO effect, int stacksToRemove)
        {
            for (int i = 0; i < stacksToRemove; i++)
            {
                effect.RemoveStack(this);
                effect.RemoveVars(this, statusEffects[effect]);
            }
            if (effectBar) { effectBar.HandleUpdateStacks(effect, statusEffects[effect].Count); }
        }

        //========== Clear ==========
        public void Clear()
        {
            foreach (var kvp in statusEffects)
            {
                RemoveEffect(kvp.Key, kvp.Value.Count);
            }
            statusEffects.Clear();
        }

        //================ Process Heal / Hurt Event ==================
        //Process Take Damage
        public void ProcessTakeDamage(ref HitEvent hitEvent, ITakeDamageProcessor processor)
        {
            processor.ProcessTakeDamage(ref hitEvent, statusEffects[processor as StatusEffectSO]);
        }

        //Process Deal Damage
        public void ProcessDealDamage(ref HitEvent hitEvent, IDealDamageProcessor processor)
        {
            processor.ProcessDealDamage(ref hitEvent, statusEffects[processor as StatusEffectSO]);
        }

        //Process Heal
        public void ProcessHeal(ref HealEvent healEvent, IHealProcessor processor)
        {
            processor.ProcessHeal(ref healEvent, statusEffects[processor as StatusEffectSO]);
        }
    }
}
