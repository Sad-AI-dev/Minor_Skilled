using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game {
    [CreateAssetMenu(fileName = "40Monster_Armor", menuName = "ScriptableObjects/Items/T2/40: Monster Armor", order = 240)]
    public class Item40SO : ItemDataSO, ITakeDamageProcessor
    {
        private class Item40Vars : Item.ItemVars
        {
            public float reducePercent;
        }

        [Header("Priority")]
        public int priority;
        public int GetPriority() { return priority; }

        [Header("DamageReduction settings")]
        public float reducePercent;

        //========= InitializeVars ===========
        public override void InitializeVars(Item item)
        {
            item.vars = new Item40Vars();
        }

        //========= Manage Stacks ===========
        public override void AddStack(Item item)
        {
            Item40Vars vars = item.vars as Item40Vars;
            vars.reducePercent = CalculateReducePercent(item.stacks);
        }

        public override void RemoveStack(Item item)
        {
            Item40Vars vars = item.vars as Item40Vars;
            vars.reducePercent = CalculateReducePercent(item.stacks);
        }

        private float CalculateReducePercent(int stacks)
        {
            float percent = stacks * reducePercent;
            return percent / (percent + 1f); //log scaling
        }

        //========== Process Take Damage =============
        public void ProcessTakeDamage(ref HitEvent hitEvent, Item sourceItem)
        {
            if (!hitEvent.blocked) {
                Item40Vars vars = sourceItem.vars as Item40Vars;
                //reduce damage
                hitEvent.damageReduction += hitEvent.GetTotalDamage() * vars.reducePercent;
            }
        }
        public void ProcessTakeDamage(ref HitEvent hitEvent, List<StatusEffectHandler.EffectVars> vars) { }

        //========== Description ===========
        public override string GenerateLongDescription()
        {
            return $"Reduce incoming <color=#{HighlightColor}>Damage</color> by " +
                $"<color=#{HighlightColor}>{reducePercent * 100f}%</color> " +
                $"<color=#{StackColor}>(+{reducePercent * 100f}% per stack)</color>";
        }
    }
}
