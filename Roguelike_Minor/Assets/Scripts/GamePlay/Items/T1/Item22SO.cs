using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game {
    [CreateAssetMenu(fileName = "22Monkey's_Paw", menuName = "ScriptableObjects/Items/T1/22: Monkey's Paw", order = 122)]
    public class Item22SO : ItemDataSO
    {
        [Header("Priority")]
        public int priority;
        public int GetPriority() { return priority; }

        [Header("Money Settings")]
        public float baseMoneyMult = 0.1f;
        public float bonusMoneyMult = 0.1f;

        //========== Handle hit event =========
        public void ProcessDealDamage(ref HitEvent hitEvent, Item sourceItem)
        {
            hitEvent.onDeath.AddListener(RewardAdditionalMoney);
        }
        public void ProcessDealDamage(ref HitEvent hitEvent, List<StatusEffectHandler.EffectVars> vars) { }

        private void RewardAdditionalMoney(HitEvent hitEvent)
        {
            Item sourceItem = hitEvent.source.inventory.GetItemOfType(this);
            float bonusMoney = hitEvent.target.agent.stats.Money * GetTotalMoneyMult(sourceItem);
            hitEvent.source.stats.Money += Mathf.FloorToInt(bonusMoney);
        }
        private float GetTotalMoneyMult(Item sourceItem)
        {
            return baseMoneyMult + bonusMoneyMult * (sourceItem.stacks - 1);
        }

        //========== Description ===========
        public override string GenerateLongDescription()
        {
            return $"Gain +<color=#{HighlightColor}>{baseMoneyMult * 100}%</color> " +
                $"<color=#{StackColor}>(+{bonusMoneyMult * 100}% per stack)</color> " +
                $"money on kills";
        }
    }
}
