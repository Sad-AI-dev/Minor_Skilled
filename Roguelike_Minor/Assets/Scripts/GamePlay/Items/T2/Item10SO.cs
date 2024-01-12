using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game {
    [CreateAssetMenu(fileName = "10Two_Right_Hands", menuName = "ScriptableObjects/Items/T2/10: Two Right Hands", order = 210)]
    public class Item10SO : ItemDataSO
    {
        [Header("Upgrade Settings")]
        public int baseUpgrades = 1;
        public int bonusUpgrades = 1;

        //========= Manage Stacks ===========
        public override void AddStack(Item item)
        {
            int levels = item.stacks == 1 ? baseUpgrades : bonusUpgrades;
            //add upgrades
            for (int i = 0; i < levels; i++)
            {
                item.agent.abilities.secondary.abilityData.Upgrade(item.agent.abilities.secondary);
            }
        }

        public override void RemoveStack(Item item)
        {
            int levels = item.stacks == 0 ? baseUpgrades : bonusUpgrades;
            //remove upgrades
            for (int i = 0; i < levels; i++)
            {
                item.agent.abilities.secondary.abilityData.DownGrade(item.agent.abilities.secondary);
            }
        }

        //========== Description ===========
        public override string GenerateLongDescription()
        {
            return $"Upgrade <color=#{HighlightColor}>Secondary Ability</color> by" +
                $" <color=#{HighlightColor}>{baseUpgrades} Level</color> " +
                $"<color=#{StackColor}>(+{bonusUpgrades} level per stack)</color>";
        }
    }
}
