using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game {
    [CreateAssetMenu(fileName = "1Necrotic_Ammunition", menuName = "ScriptableObjects/Items/T1/1: Necrotic Ammunition", order = 101)]
    public class Item1SO : ItemDataSO, IDealDamageProcessor
    {
        [Header("Priority")]
        public int priority;
        public int GetPriority() { return priority; }

        [Header("Healing settings")]
        public float baseHeal = 2f;
        public float bonusHeal = 1f;

        //============ Process hit / heal events ==============
        public void ProcessDealDamage(ref HitEvent hitEvent, Item sourceItem)
        {
            hitEvent.onDeath.AddListener(OnEnemyDeath);
        }
        public void ProcessDealDamage(ref HitEvent hitEvent, List<StatusEffectHandler.EffectVars> vars) { }

        //============ Handle Enemy Kill ============
        private void OnEnemyDeath(HitEvent hitEvent)
        {
            //get stacks
            int stacks = hitEvent.source.inventory.GetItemOfType(this).stacks - 1;
            //heal agent
            HealEvent toHeal = new HealEvent(baseHeal + bonusHeal * stacks);
            hitEvent.source.health.Heal(toHeal);
        }

        //============ Description ===========
        public override string GenerateLongDescription()
        {
            return $"Killing an enemy <color=#{HighlightColor}>Heals</color> you for " +
                $"<color=#{HighlightColor}>{baseHeal}HP</color> " +
                $"<color=#{StackColor}>(+{bonusHeal}HP per stack)</color>";
        }
    }
}
