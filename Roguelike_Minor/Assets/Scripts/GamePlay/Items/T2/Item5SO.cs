using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

//shorthand
using DoomVars = Game.DelayDamageEffectSO.DelayDamageEffectVars;

namespace Game {
    [CreateAssetMenu(fileName = "5Special_Surprise", menuName = "ScriptableObjects/Items/T2/5: Special Surprise", order = 205)]
    public class Item5SO : ItemDataSO, IDealDamageProcessor
    {
        private class Item5Vars : Item.ItemVars
        {
            public float damageMult;
        }

        [Header("Priority")]
        public int priority;
        public int GetPriority() { return priority; }

        [Header("Chance settings")]
        public float chance = 20f;

        [Header("Doom Settings")]
        public DelayDamageEffectSO doomEffect;
        public float baseDamageMult = 1f;
        public float bonusDamageMult = 0.5f;
        public float procCoef = 0f;

        //========= Initialize Vars ==========
        public override void InitializeVars(Item item)
        {
            item.vars = new Item5Vars();
        }

        //========= Manage Stacks ===========
        public override void AddStack(Item item)
        {
            Item5Vars vars = item.vars as Item5Vars;
            if (item.stacks == 1) { vars.damageMult += baseDamageMult; }
            else { vars.damageMult += bonusDamageMult; }
        }

        public override void RemoveStack(Item item)
        {
            Item5Vars vars = item.vars as Item5Vars;
            if (item.stacks == 0) { vars.damageMult -= baseDamageMult; }
            else { vars.damageMult -= bonusDamageMult; }
        }

        //======== Handle deal damage ==============
        public void ProcessDealDamage(ref HitEvent hitEvent, Item sourceItem)
        {
            HitEvent copyEvent = hitEvent;
            AgentRandom.TryProc(chance, hitEvent, () => InflictDoom(copyEvent, sourceItem));
        }
        public void ProcessDealDamage(ref HitEvent hitEvent, List<StatusEffectHandler.EffectVars> vars) { }

        private void InflictDoom(HitEvent hitEvent, Item sourceItem)
        {
            //apply effect
            StatusEffectHandler target = hitEvent.target.agent.effectHandler;
            DoomVars effectVars = target.AddEffect(doomEffect) as DoomVars;
            //setup effect vars
            effectVars.sourceEvent = new HitEvent(hitEvent, sourceItem, procCoef);
            effectVars.damageMult = (sourceItem.vars as Item5Vars).damageMult;
        }

        //========== Description ===========
        public override string GenerateLongDescription()
        {
            return $"On hit, gain a <color=#{HighlightColor}>{chance}% chance</color> " +
                $"to inflict <color=#{HighlightColor}>Doom</color>\n" +
                $"Dealing <color=#{HighlightColor}>{baseDamageMult * 100}%</color> " +
                $"<color=#{StackColor}>(+{bonusDamageMult * 100}% per stack)</color> " +
                $"total damage after a short delay";
        }
    }
}
