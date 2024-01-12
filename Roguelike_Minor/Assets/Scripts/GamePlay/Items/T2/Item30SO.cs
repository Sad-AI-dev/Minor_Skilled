using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Game.Core;

using EffectVars = Game.VulnerableEffectSO.VulnerableVars;

namespace Game {
    [CreateAssetMenu(fileName = "30Critical_Squid", menuName = "ScriptableObjects/Items/T2/30: Critical Squid", order = 230)]
    public class Item30SO : ItemDataSO, IDealDamageProcessor
    {
        private class Item30Vars : Item.ItemVars
        {
            public float damageMult;
        }

        [Header("Priority")]
        public int priority;
        public int GetPriority() { return priority; }

        [Header("Crit Settings")]
        public float critChance = 5f;

        [Header("Effect Settings")]
        public float baseDamageMult = 1f;
        public float bonusDamageMult = 1f;
        public VulnerableEffectSO effect;

        //========= Initialize vars =========
        public override void InitializeVars(Item item)
        {
            item.vars = new Item30Vars();
        }

        //========= Manage Stacks ===========
        public override void AddStack(Item item)
        {
            Item30Vars vars = item.vars as Item30Vars;
            if (item.stacks == 1) 
            { 
                vars.damageMult += baseDamageMult;
                item.agent.stats.critChance += critChance;
            }
            else { vars.damageMult += bonusDamageMult; }
        }

        public override void RemoveStack(Item item)
        {
            Item30Vars vars = item.vars as Item30Vars;
            if (item.stacks == 0) 
            { 
                vars.damageMult -= baseDamageMult;
                item.agent.stats.critChance -= critChance;
            }
            else { vars.damageMult -= bonusDamageMult; }
        }

        //========== Handle Hit Event ==========
        public void ProcessDealDamage(ref HitEvent hitEvent, Item sourceItem)
        {
            if (hitEvent.isCrit && TargetIsValid(hitEvent))
            {
                ApplyEffect(hitEvent, sourceItem);
            }
        }
        public void ProcessDealDamage(ref HitEvent hitEvent, List<StatusEffectHandler.EffectVars> vars) { }

        private bool TargetIsValid(HitEvent hitEvent)
        {
            StatusEffectHandler handler = hitEvent.target.agent.effectHandler;
            if (handler.statusEffects.ContainsKey(effect))
            {
                foreach (EffectVars vars in handler.statusEffects[effect].Cast<EffectVars>()) 
                {
                    if (vars.source == this) { return false; }
                }
            }
            return true;
        }
        private void ApplyEffect(HitEvent hitEvent, Item sourceItem)
        {
            EffectVars vars = hitEvent.target.agent.effectHandler.AddEffect(effect) as EffectVars;
            vars.source = this;
            vars.damageMult = (sourceItem.vars as Item30Vars).damageMult;
        }

        //========== Description ===========
        public override string GenerateLongDescription()
        {
            return $"Crits inflict <color=#{HighlightColor}>Vulnerable</color> " +
                $"<color=#{HighlightColor}>+{baseDamageMult * 100}%</color> " +
                $"<color=#{StackColor}>(+{bonusDamageMult * 100}% per stack)</color> all-source damage to enemies " +
                $"for <color=#{HighlightColor}>{effect.duration} </color> seconds " +
                $"& increases <color=#{HighlightColor}>Crit Chance</color> by" +
                $" <color=#{HighlightColor}>{critChance}%</color>";
        }
    }
}
