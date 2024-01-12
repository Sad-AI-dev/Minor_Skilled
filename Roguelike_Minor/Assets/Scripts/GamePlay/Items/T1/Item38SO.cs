using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game {
    [CreateAssetMenu(fileName = "38Homemade_Soup", menuName = "ScriptableObjects/Items/T1/38: Homemade Soup", order = 138)]
    public class Item38SO : ItemDataSO
    {
        [Header("Regen settings")]
        public float baseRegen = 3f;
        public float bonusRegen = 3f;

        //========= Manage Stacks ===========
        public override void AddStack(Item item)
        {
            if (item.stacks == 1) { item.agent.stats.regeneration += baseRegen; }
            else { item.agent.stats.regeneration += bonusRegen; }
        }

        public override void RemoveStack(Item item)
        {
            if (item.stacks == 0) { item.agent.stats.regeneration -= baseRegen; }
            else { item.agent.stats.regeneration -= bonusRegen; }
        }

        //========== Description ===========
        public override string GenerateLongDescription()
        {
            return $"<color=#{HighlightColor}>+{baseRegen}</color> " +
                $"<color=#{StackColor}>(+{bonusRegen} per stack)</color> " +
                $"<color=#{HighlightColor}>Health Regeneration</color> " +
                $"per second";
        }
    }
}
