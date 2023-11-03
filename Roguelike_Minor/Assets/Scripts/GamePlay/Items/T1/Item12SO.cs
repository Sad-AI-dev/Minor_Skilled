using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game {
    [CreateAssetMenu(fileName = "HeartRoot", menuName = "ScriptableObjects/Items/T1/12: HeartRoot", order = 112)]
    public class Item12SO : ItemDataSO
    {
        [Header("Health settings")]
        public float baseHealth = 0.1f;
        public float bonusHealth = 0.1f;

        //========= Manage Stacks ===========
        public override void AddStack(Item item)
        {
            float oldMaxHealth = item.agent.stats.GetMaxHealth();
            if (item.stacks == 1) { item.agent.stats.maxHealthMult += baseHealth; }
            else { item.agent.stats.maxHealthMult += bonusHealth; }

            //heal to compensate for increased max health
            float toHeal = item.agent.stats.GetMaxHealth() - oldMaxHealth;
            item.agent.health.Heal(new HealEvent(toHeal));
            item.agent.health.onMaxHealthChanged?.Invoke();
        }

        public override void RemoveStack(Item item)
        {
            if (item.stacks == 0) { item.agent.stats.maxHealthMult -= baseHealth; }
            else { item.agent.stats.maxHealthMult -= bonusHealth; }

            //update health manager
            item.agent.health.onMaxHealthChanged?.Invoke();
        }

        //========== Description ===========
        public override string GenerateLongDescription()
        {
            return $"Increase <color=#{HighlightColor}>max health</color> by" +
                $" <color=#{HighlightColor}>{baseHealth * 100}%</color> " +
                $"<color=#{StackColor}>(+{bonusHealth * 100}% per stack)</color>";
        }
    }
}
