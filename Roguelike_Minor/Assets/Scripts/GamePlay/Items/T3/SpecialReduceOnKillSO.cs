using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game {
    [CreateAssetMenu(fileName = "SpecialReduceOnKill", menuName = "ScriptableObjects/Items/T3/SpecialReduceOnKill")]
    public class SpecialReduceOnKillSO : ItemDataSO
    {
        private static readonly string reduction = "reduction";
        
        public float cooldownReduction = 1f;
        public float stackBonusReduction = 1f;

        //========= Manage Stacks ===========
        public override void AddStack(Item item) 
        {
            if (!item.vars.ContainsKey(reduction)) { InitializeVars(item); }
            SetReduction(cooldownReduction + (stackBonusReduction * (item.stacks - 1)), item);
        }

        public override void RemoveStack(Item item) 
        {
            SetReduction(cooldownReduction + (stackBonusReduction * (item.stacks - 1)), item);
        }

        //========= Process Hit Events ===========
        public override void ProcessDealDamage(ref HitEvent hitEvent) 
        {
            hitEvent.onDeath.AddListener(ReduceSpecialCooldown);
        }

        public override void ProcessTakeDamage(ref HitEvent hitEvent) { }

        //========= Process Heal Events ============
        public override void ProcessHealEvent(ref HealEvent healEvent) { }

        //====== Handle Enemy Death =======
        private void ReduceSpecialCooldown(HitEvent hitEvent)
        {
            hitEvent.source.abilities.special.ReduceCoolDown(
                GetReduction(hitEvent.source.inventory.GetItemOfType(this))
            );
        }

        //====== Util =======
        private void InitializeVars(Item item)
        {
            item.vars.Add(reduction, 0.0f);
        }

        private float GetReduction(Item item)
        {
            return (float)item.vars[reduction];
        }
        private void SetReduction(float toSet, Item item)
        {
            item.vars[reduction] = toSet;
        }
    }
}
