using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game {
    [CreateAssetMenu(fileName = "DeathHealItem", menuName = "ScriptableObjects/Items/T1/Death Heal")]
    public class DeathHealItemSO : ItemDataSO
    {
        [Header("Healing settings")]
        public float baseHeal = 5f;
        public float bonusHeal = 2.5f;

        //============ Manage Stacks ===============
        public override void AddStack(Item item) { }
        public override void RemoveStack(Item item) { }

        //============ Process hit / heal events ==============
        public override void ProcessDealDamage(ref HitEvent hitEvent)
        {
            hitEvent.onDeath.AddListener(OnEnemyDeath);
        }

        public override void ProcessTakeDamage(ref HitEvent hitEvent) { }
        public override void ProcessHealEvent(ref HealEvent healEvent) { }

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
            return $"Killing an enemy heals you for {baseHeal}hp (+{bonusHeal}hp per stack)";
        }
    }
}
