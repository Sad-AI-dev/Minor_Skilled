using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game {
    [CreateAssetMenu(fileName = "31Utility_Goggles", menuName = "ScriptableObjects/Items/T2/31: Utility Goggles", order = 231)]
    public class Item31SO : ItemDataSO
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
                item.agent.abilities.utility.abilityData.Upgrade(item.agent.abilities.utility);
            }
        }

        public override void RemoveStack(Item item)
        {
            int levels = item.stacks == 0 ? baseUpgrades : bonusUpgrades;
            //remove upgrades
            for (int i = 0; i < levels; i++)
            {
                item.agent.abilities.utility.abilityData.DownGrade(item.agent.abilities.utility);
            }
        }

        //========== Description ===========
        public override string GenerateLongDescription()
        {
            return $"Upgrade <color=#{HighlightColor}>Utility Ability</color> by" +
                $" <color=#{HighlightColor}>{baseUpgrades} Level</color> " +
                $"<color=#{StackColor}>(+{bonusUpgrades} level per stack)</color>";
        }
    }
}
