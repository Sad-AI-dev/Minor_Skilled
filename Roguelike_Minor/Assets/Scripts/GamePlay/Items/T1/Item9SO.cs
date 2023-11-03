using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game {
    [CreateAssetMenu(fileName = "Crit-shroom", menuName = "ScriptableObjects/Items/T1/9: Crit-shroom", order = 109)]
    public class Item9SO : ItemDataSO
    {
        [Header("Crit Chance settings")]
        public float baseCritChance = 10;
        public float bonusCritChance = 10;

        [Header("Crit Damage settings")]
        public float baseCritDmg = 0.1f;
        public float bonusCritDmg = 0.05f;

        //========= Manage Stacks ===========
        public override void AddStack(Item item)
        {
            if (item.stacks == 1) 
            { 
                item.agent.stats.critChance += baseCritChance;
                item.agent.stats.critMult += baseCritDmg;
            }
            else 
            { 
                item.agent.stats.critChance += bonusCritChance;
                item.agent.stats.critMult += bonusCritDmg;
            }
        }

        public override void RemoveStack(Item item)
        {
            if (item.stacks == 0) 
            { 
                item.agent.stats.critChance -= baseCritChance;
                item.agent.stats.critMult -= baseCritDmg;
            }
            else
            { 
                item.agent.stats.critChance -= bonusCritChance;
                item.agent.stats.critMult -= bonusCritDmg;
            }
        }

        //========== Description ===========
        public override string GenerateLongDescription()
        {
            return $"Increases <color=#{HighlightColor}>crit chance</color> by " +
                $"<color=#{HighlightColor}>{baseCritChance}%</color> " +
                $"<color=#{StackColor}>(+{bonusCritChance}% per stack)</color> and " +
                $"<color=#{HighlightColor}>crit damage</color> by " +
                $"<color=#{HighlightColor}>{baseCritDmg * 100}%</color> " +
                $"<color=#{StackColor}>(+{bonusCritDmg * 100}% per stack)</color>";
        }
    }
}
