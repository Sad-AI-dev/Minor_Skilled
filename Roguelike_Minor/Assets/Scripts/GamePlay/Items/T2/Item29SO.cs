using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game {
    [CreateAssetMenu(fileName = "29Timmy", menuName = "ScriptableObjects/Items/T2/29: Timmy", order = 229)]
    public class Item29SO : ItemDataSO, IDealDamageProcessor
    {
        private class Item29Vars : Item.ItemVars
        {
            public float damageMult;
        }

        [Header("Priority")]
        public int priority;
        public int GetPriority() { return priority; }

        [Header("Damage Settings")]
        public float baseDamageMult = 0.2f;
        public float bonusDamageMult = 0.2f;

        //========= Initialize Vars ===========
        public override void InitializeVars(Item item)
        {
            item.vars = new Item29Vars();
        }

        //========= Manage Stacks ===========
        public override void AddStack(Item item)
        {
            Item29Vars vars = item.vars as Item29Vars;
            if (item.stacks == 1) { vars.damageMult += baseDamageMult; }
            else { vars.damageMult += bonusDamageMult; }
        }

        public override void RemoveStack(Item item)
        {
            Item29Vars vars = item.vars as Item29Vars;
            if (item.stacks == 0) { vars.damageMult -= baseDamageMult; }
            else { vars.damageMult -= bonusDamageMult; }
        }

        //========== Process Deal Damage =========
        public void ProcessDealDamage(ref HitEvent hitEvent, Item sourceItem)
        {
            Item29Vars vars = sourceItem.vars as Item29Vars;
            hitEvent.damageMultiplier += hitEvent.target.agent.effectHandler.statusEffects.Count * vars.damageMult;
        }
        public void ProcessDealDamage(ref HitEvent hitEvent, List<StatusEffectHandler.EffectVars> vars) { }

        //========== Description ===========
        public override string GenerateLongDescription()
        {
            return $"Deal <color=#{HighlightColor}>{baseDamageMult * 100}%</color> " +
                $"<color=#{StackColor}>(+{bonusDamageMult * 100}% per stack)</color> " +
                $"additional damage for each " +
                $"<color=#{HighlightColor}>unique status condition</color> " +
                $"on the enemy.";
        }
    }
}
