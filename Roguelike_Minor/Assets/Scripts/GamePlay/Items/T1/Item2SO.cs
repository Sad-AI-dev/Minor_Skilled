using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game {
    [CreateAssetMenu(fileName = "High_Speed_Connector", menuName = "ScriptableObjects/Items/T1/2: -", order = 102)]
    public class Item2SO : ItemDataSO
    {
        [Header("AttackSpeed Settings")]
        public float baseAttackSpeedIncrease = 1f;
        public float stackAttackSpeedIncrease = 0.5f;

        //========= Initialize Vars ============
        public override void InitializeVars(Item item) { }

        //========= Manage Stacks ===========
        public override void AddStack(Item item)
        {
            if (item.stacks == 1) { item.agent.stats.attackSpeed += baseAttackSpeedIncrease; }
            else { item.agent.stats.attackSpeed += stackAttackSpeedIncrease; }
        }

        public override void RemoveStack(Item item)
        {
            if (item.stacks == 0) { item.agent.stats.attackSpeed -= baseAttackSpeedIncrease; }
            else { item.agent.stats.attackSpeed -= stackAttackSpeedIncrease; }
        }

        //========= Process Hit Events ===========
        public override void ProcessDealDamage(ref HitEvent hitEvent, Item sourceItem) { }

        public override void ProcessTakeDamage(ref HitEvent hitEvent, Item sourceItem) { }

        //========= Process Heal Events ============
        public override void ProcessHealEvent(ref HealEvent healEvent, Item sourceItem) { }

        //========== Description ===========
        public override string GenerateLongDescription()
        {
            return $"Increase <color=#{HighlightColor}>attack speed</color> by" +
                $" <color=#{HighlightColor}>{baseAttackSpeedIncrease * 100}%</color> " +
                $"<color=#{StackColor}>(+{stackAttackSpeedIncrease * 100}% per stack)</color>";
        }
    }
}