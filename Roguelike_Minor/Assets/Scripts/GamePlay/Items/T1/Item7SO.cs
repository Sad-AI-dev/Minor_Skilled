using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game {
    [CreateAssetMenu(fileName = "7Propeller_Hat", menuName = "ScriptableObjects/Items/T1/7: Propeller Hat", order = 107)]
    public class Item7SO : ItemDataSO
    {
        private class Item7Vars : Item.ItemVars
        {
            public int stacksToCreate = 0;
        }

        [Header("Status Effect settings")]
        public AttackSpeedEffectSO statusEffect;
        public int baseEffectStacks = 2;
        public int bonusEffectStacks = 1;

        //========= Initialize Vars ============
        public override void InitializeVars(Item item)
        {
            item.vars = new Item7Vars();
        }

        //========= Manage Stacks ===========
        public override void AddStack(Item item)
        {
            Item7Vars vars = item.vars as Item7Vars;
            if (item.stacks == 1)
            {
                item.agent.abilities.utility.onUse.AddListener(OnUseUtil);
                vars.stacksToCreate += baseEffectStacks;
            }
            else { vars.stacksToCreate += bonusEffectStacks; }
        }

        public override void RemoveStack(Item item)
        {
            Item7Vars vars = item.vars as Item7Vars;
            if (item.stacks == 0)
            {
                item.agent.abilities.utility.onUse.RemoveListener(OnUseUtil);
                vars.stacksToCreate -= baseEffectStacks;
            }
            else { vars.stacksToCreate -= bonusEffectStacks; }
        }

        //========== Handle Util Use ===============
        private void OnUseUtil(Ability source)
        {
            Item7Vars vars = source.agent.inventory.GetItemOfType(this).vars as Item7Vars;
            source.agent.effectHandler.AddEffect(statusEffect, vars.stacksToCreate);
        }

        //========== Description ===========
        public override string GenerateLongDescription()
        {
            return $"Increase <color=#{HighlightColor}>attack speed</color> by" +
                $" <color=#{HighlightColor}>{(baseEffectStacks * statusEffect.attackSpeedIncrease) * 100}%</color> " +
                $"<color=#{StackColor}>(+{(bonusEffectStacks * statusEffect.attackSpeedIncrease) * 100}% per stack)</color>" +
                $"for <color=#{HighlightColor}>{statusEffect.duration} seconds</color> on " +
                $"<color=#{HighlightColor}>Util</color> use.";
        }
    }
}
