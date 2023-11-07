using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game {
    [CreateAssetMenu(fileName = "37Lucky_Batwing", menuName = "ScriptableObjects/Items/T3/37: Lucky Batwing", order = 337)]
    public class Item37SO : ItemDataSO
    {
        [Header("MoveSpeed settings")]
        public int baseLuck = 1;
        public int bonusLuck = 1;

        //========= Manage Stacks ===========
        public override void AddStack(Item item)
        {
            if (item.stacks == 1) { item.agent.stats.luck += baseLuck; }
            else { item.agent.stats.luck += bonusLuck; }
        }

        public override void RemoveStack(Item item)
        {
            if (item.stacks == 0) { item.agent.stats.luck -= baseLuck; }
            else { item.agent.stats.luck -= bonusLuck; }
        }

        //========== Description ===========
        public override string GenerateLongDescription()
        {
            return $"gain <color=#{HighlightColor}>{baseLuck}</color> " +
                $"<color=#{StackColor}>(+{bonusLuck} per stack)</color> " +
                $"<color=#{HighlightColor}>luck</color>.";
        }
    }
}
