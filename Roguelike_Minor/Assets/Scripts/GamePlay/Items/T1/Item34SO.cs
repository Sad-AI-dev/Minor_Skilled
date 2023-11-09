using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game {
    [CreateAssetMenu(fileName = "34Unruly_Equalizer", menuName = "ScriptableObjects/Items/T1/34: Unruly Equalizer", order = 134)]
    public class Item34SO : ItemDataSO
    {
        [Header("Damage Settings")]
        public float baseDamageMult = 0.2f;
        public float bonusDamageMult = 0.2f;

        [Header("Trigger Settings")]
        public float triggerPercent = 0.9f;

        [Header("UI visuals Settings")]
        public Color damageColor;
        public Color critColor;

        //=========== Handle Hit Event ============
        public override void ProcessDealDamage(ref HitEvent hitEvent, Item sourceItem)
        {
            if (hitEvent.target.health / hitEvent.target.agent.stats.GetMaxHealth() > triggerPercent)
            {
                hitEvent.damageMultiplier += GetDamageMult(sourceItem);
                hitEvent.labelColor = damageColor;
                hitEvent.critColor = critColor;
            }
        }

        private float GetDamageMult(Item sourceItem)
        {
            return baseDamageMult + bonusDamageMult * (sourceItem.stacks - 1);
        }

        //========== Description ===========
        public override string GenerateLongDescription()
        {
            return $"Deal <color=#{HighlightColor}>{baseDamageMult * 100}%</color> " +
                $"<color=#{StackColor}>(+{bonusDamageMult * 100}% per stack)</color> " +
                $"additional <color=#{HighlightColor}>damage</color> to enemies " +
                $"above <color=#{HighlightColor}>{triggerPercent * 100}%</color> health";
        }
    }
}
