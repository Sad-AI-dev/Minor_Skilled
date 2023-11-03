using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game {
    [CreateAssetMenu(fileName = "Overclock", menuName = "ScriptableObjects/Items/T3/4: Overclock", order = 304)]
    public class Item4SO : ItemDataSO
    {
        [Header("Cooldown settings")]
        public float cooldownReduction = 1f;
        public float stackBonusReduction = 1f;

        //========= Process Hit Events ===========
        public override void ProcessDealDamage(ref HitEvent hitEvent, Item sourceItem) 
        {
            hitEvent.onDeath.AddListener(ReduceSpecialCooldown);
        }

        //====== Handle Enemy Death =======
        private void ReduceSpecialCooldown(HitEvent hitEvent)
        {
            hitEvent.source.abilities.special.ReduceCoolDown(
                GetReduction(hitEvent.source.inventory.GetItemOfType(this))
            );
        }

        //====== Util =======
        private float GetReduction(Item item)
        {
            return cooldownReduction + (stackBonusReduction * (item.stacks - 1));
        }

        //============ Description ================
        public override string GenerateLongDescription()
        {
            return $"Killing an enemy <color=#{HighlightColor}>reduces special cooldown</color> " +
                $"by <color=#{HighlightColor}>{cooldownReduction} seconds</color> " +
                $"<color=#{StackColor}>(+{stackBonusReduction} seconds per stack)</color>";
        }
    }
}
