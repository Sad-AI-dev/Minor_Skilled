using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game {
    [CreateAssetMenu(fileName = "Necrotic_Module", menuName = "ScriptableObjects/Items/T1/1: -", order = 101)]
    public class Item1SO : ItemDataSO
    {
        [Header("Healing settings")]
        public float baseHeal = 2f;
        public float bonusHeal = 1f;
        
        //========= Initialize Vars ============
        public override void InitializeVars(Item item) { }

        //============ Manage Stacks ===============
        public override void AddStack(Item item) { }
        public override void RemoveStack(Item item) { }

        //============ Process hit / heal events ==============
        public override void ProcessDealDamage(ref HitEvent hitEvent, Item sourceItem)
        {
            hitEvent.onDeath.AddListener(OnEnemyDeath);
        }

        public override void ProcessTakeDamage(ref HitEvent hitEvent, Item sourceItem) { }
        public override void ProcessHealEvent(ref HealEvent healEvent, Item sourceItem) { }

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
            return $"Killing an enemy <color=#{HighlightColor}>heals</color> you for " +
                $"<color=#{HighlightColor}>{baseHeal}hp</color> " +
                $"<color=#{StackColor}>(+{bonusHeal}hp per stack)</color>";
        }
    }
}
