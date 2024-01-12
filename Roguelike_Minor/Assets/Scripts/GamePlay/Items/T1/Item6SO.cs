using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game {
    [CreateAssetMenu(fileName = "6Hat_Of_Speed", menuName = "ScriptableObjects/Items/T1/6: Hat of Speed", order = 106)]
    public class Item6SO : ItemDataSO
    {
        [Header("MoveSpeed settings")]
        public float baseSpeedIncrease = 3f;
        public float bonusSpeedIncrease = 3f;

        //========= Manage Stacks ===========
        public override void AddStack(Item item)
        {
            if (item.stacks == 1) { item.agent.stats.walkSpeed += baseSpeedIncrease; }
            else { item.agent.stats.walkSpeed += bonusSpeedIncrease; }
        }

        public override void RemoveStack(Item item)
        {
            if (item.stacks == 0) { item.agent.stats.walkSpeed -= baseSpeedIncrease; }
            else { item.agent.stats.walkSpeed -= bonusSpeedIncrease; }
        }

        //========== Description ===========
        public override string GenerateLongDescription()
        {
            return $"Increase <color=#{HighlightColor}>Move Speed</color> by" +
                $" <color=#{HighlightColor}>{baseSpeedIncrease}</color> " +
                $"<color=#{StackColor}>(+{bonusSpeedIncrease} per stack)</color>";
        }
    }
}
