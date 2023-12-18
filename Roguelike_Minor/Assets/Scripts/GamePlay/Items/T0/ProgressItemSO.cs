using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game {
    [CreateAssetMenu(fileName = "-1ProgressItem", menuName = "ScriptableObjects/Items/T0/-1: Progress Item", order = -001)]
    public class ProgressItemSO : ItemDataSO
    {
        [Header("Health Settings")]
        public float maxHealth = 10f;

        [Header("Damage Settings")]
        public float damage = 1f;

        //========= Manage Stacks ===========
        public override void AddStack(Item item)
        {
            item.agent.stats.baseDamage += damage;
            //increase max health
            float oldMaxHealth = item.agent.stats.GetMaxHealth();
            item.agent.stats.maxHealth += maxHealth;
            float toHeal = item.agent.stats.GetMaxHealth() - oldMaxHealth;
            item.agent.health.Heal(new HealEvent(toHeal));
        }

        public override void RemoveStack(Item item)
        {
            item.agent.stats.baseDamage -= damage;
            item.agent.stats.maxHealth -= maxHealth;
        }

        //========== Description ===========
        public override string GenerateLongDescription()
        {
            return $"This item should be invisible.\n" +
                $"If you are seeing this, let Arvid know something has gone very, very wrong.\n" +
                $"pain";
        }
    }
}
