using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game {
    [CreateAssetMenu(fileName = "8DPS Drive", menuName = "ScriptableObjects/Items/T1/8: DPS Drive", order = 108)]
    public class Item8SO : ItemDataSO
    {
        [Header("Base Damage settings")]
        public float baseDamageIncrease = 4f;
        public float bonusDamageIncrease = 2f;

        //========= Manage Stacks ===========
        public override void AddStack(Item item)
        {
            if (item.stacks == 1) { item.agent.stats.baseDamage += baseDamageIncrease; }
            else { item.agent.stats.baseDamage += bonusDamageIncrease; }
        }

        public override void RemoveStack(Item item)
        {
            if (item.stacks == 0) { item.agent.stats.baseDamage -= baseDamageIncrease; }
            else { item.agent.stats.baseDamage -= bonusDamageIncrease; }
        }

        //========== Description ===========
        public override string GenerateLongDescription()
        {
            return $"Increase <color=#{HighlightColor}>Base Damage</color> by" +
                $" <color=#{HighlightColor}>{baseDamageIncrease}</color> " +
                $"<color=#{StackColor}>(+{bonusDamageIncrease} per stack)</color>";
        }
    }
}
