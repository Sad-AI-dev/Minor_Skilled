using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game {
    [CreateAssetMenu(fileName = "4Overclock", menuName = "ScriptableObjects/Items/T3/4: Overclock", order = 304)]
    public class Item4SO : ItemDataSO, IDealDamageProcessor
    {
        [Header("Priority")]
        public int priority;
        public int GetPriority() { return priority; }

        [Header("Stack Settings")]
        public int baseUsesIncrease = 0;
        public int bonusUseIncrease = 1;

        //========= Manage Stacks ===========
        public override void AddStack(Item item)
        {
            if (item.stacks != 1)
            {
                item.agent.abilities.special.GainMaxUses(1);
            }
        }

        public override void RemoveStack(Item item)
        {
            if (item.stacks != 0)
            {
                item.agent.abilities.special.RemoveMaxUses(1);
            }
        }

        //========= Process Hit Events ===========
        public void ProcessDealDamage(ref HitEvent hitEvent, Item sourceItem)
        {
            hitEvent.onDeath.AddListener(GainSpecialUse);
        }
        public void ProcessDealDamage(ref HitEvent hitEvent, List<StatusEffectHandler.EffectVars> vars) { }

        //====== Handle Enemy Death =======
        private void GainSpecialUse(HitEvent hitEvent)
        {
            hitEvent.source.abilities.special.GainUses(1);
        }

        //============ Description ================
        public override string GenerateLongDescription()
        {
            return $"Killing an enemy <color=#{HighlightColor}>restores 1 special use</color> " +
                $"<color=#{StackColor}>(+{bonusUseIncrease} max special use per stack)</color>";
        }
    }
}
