using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using System;

namespace Game {
    [CreateAssetMenu(fileName = "16Spiked_Bracers", menuName = "ScriptableObjects/Items/T1/16: Spiked Bracers", order = 116)]
    public class Item16SO : ItemDataSO, IDealDamageProcessor
    {
        private class Item16Vars : Item.ItemVars
        {
            public float bleedChance;
        }

        [Header("Priority")]
        public int priority;
        public int GetPriority() { return priority; }

        [Header("Bleed settings")]
        public float baseChance = 10f;
        public float bonusChance = 10f;

        [Header("Effect settings")]
        public DOTEffect bleedEffect;
        public float bleedDamageMult = 0.5f;

        public override void InitializeVars(Item item)
        {
            item.vars = new Item16Vars();
        }

        //========= Manage Stacks ===========
        public override void AddStack(Item item)
        {
            Item16Vars vars = item.vars as Item16Vars;
            if (item.stacks == 1) { vars.bleedChance += baseChance; }
            else { vars.bleedChance += bonusChance; }
        }

        public override void RemoveStack(Item item)
        {
            Item16Vars vars = item.vars as Item16Vars;
            if (item.stacks == 0) { vars.bleedChance -= baseChance; }
            else { vars.bleedChance -= bonusChance; }
        }

        //=========== Process Hit Event ==========
        public void ProcessDealDamage(ref HitEvent hitEvent, Item item)
        {
            Item16Vars vars = item.vars as Item16Vars;
            AgentRandom.TryProc(vars.bleedChance, hitEvent, ApplyBleed, hitEvent);
        }
        public void ProcessDealDamage(ref HitEvent hitEvent, List<StatusEffectHandler.EffectVars> vars) { }

        private void ApplyBleed(HitEvent hitEvent)
        {
            StatusEffectHandler effectHandler = hitEvent.target.agent.effectHandler;
            DOTEffect.DOTEffectVars vars = effectHandler.AddEffect(bleedEffect) as DOTEffect.DOTEffectVars;
            //initialize effect vars
            vars.dmg = bleedDamageMult * hitEvent.source.stats.baseDamage;
            vars.source = hitEvent.source;
        }

        //========== Description ===========
        public override string GenerateLongDescription()
        {
            return $"Adds a <color=#{HighlightColor}>{baseChance}%</color> " +
                $"<color=#{StackColor}>(+{bonusChance}% per stack)</color> " +
                $"chance to inflict <color=#{HighlightColor}>Bleed</color> on hit, dealing " +
                $"<color=#{HighlightColor}>{bleedEffect.GetTotalTicks() * bleedDamageMult * 100}% " +
                $"Base Damage</color> over " +
                $"<color=#{HighlightColor}>{bleedEffect.duration} seconds</color>";
        }
    }
}
