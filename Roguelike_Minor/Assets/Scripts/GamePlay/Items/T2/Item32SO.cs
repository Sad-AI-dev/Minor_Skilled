using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Core.GameSystems;

using EffectVars = Game.Core.StatusEffectHandler.EffectVars;

namespace Game {
    [CreateAssetMenu(fileName = "32Contagious_Fang", menuName = "ScriptableObjects/Items/T2/32: Contagious Fang", order = 232)]
    public class Item32SO : ItemDataSO, IDealDamageProcessor
    {
        private class Item32Vars : Item.ItemVars
        {
            public float range;
        }

        [Header("Priority")]
        public int priority;
        public int GetPriority() { return priority; }

        [Header("Radius settings")]
        public float baseRadius = 3f;
        public float bonusRadius = 3f;

        //========= Initialize Vars ===========
        public override void InitializeVars(Item item)
        {
            item.vars = new Item32Vars();
        }

        //========= Manage Stacks ===========
        public override void AddStack(Item item)
        {
            Item32Vars vars = item.vars as Item32Vars;
            if (item.stacks == 1) { vars.range += baseRadius; }
            else { vars.range += bonusRadius; }
        }

        public override void RemoveStack(Item item)
        {
            Item32Vars vars = item.vars as Item32Vars;
            if (item.stacks == 0) { vars.range -= baseRadius; }
            else { vars.range -= bonusRadius; }
        }

        //========== Process Deal Damage ===========
        public void ProcessDealDamage(ref HitEvent hitEvent, Item sourceItem)
        {
            hitEvent.onDeath.AddListener(SpreadEffects);
        }
        public void ProcessDealDamage(ref HitEvent hitevent, List<EffectVars> vars) { }

        //========= Spread status effects =========
        private void SpreadEffects(HitEvent hitEvent)
        {
            //cache vars
            Item32Vars vars = hitEvent.source.inventory.GetItemOfType(this).vars as Item32Vars;
            //copy status effects
            Dictionary<StatusEffectSO, List<EffectVars>> effectsToSpread = new Dictionary<StatusEffectSO, List<EffectVars>>(
                hitEvent.target.agent.effectHandler.statusEffects
            );
            //find targets
            List<Agent> targets = Explosion.FindAgentsInRange(hitEvent.target.transform.position, vars.range, hitEvent.source);
            //prune list
            if (targets.Contains(hitEvent.target.agent)) { targets.Remove(hitEvent.target.agent); }
            //copy effects
            targets.ForEach((Agent target) => CopyEffects(effectsToSpread, target));
        }

        private void CopyEffects(Dictionary<StatusEffectSO, List<EffectVars>> effectsToSpread, Agent target)
        {
            foreach (var pair in effectsToSpread)
            {
                foreach (EffectVars vars in pair.Value)
                {
                    EffectVars newVars = target.effectHandler.AddEffect(pair.Key);
                    newVars.Copy(vars);
                }
            }
        }

        //========== Description ===========
        public override string GenerateLongDescription()
        {
            return $"On death, enemies " +
                $"<color=#{HighlightColor}>spread status conditions</color> " +
                $"in a <color=#{HighlightColor}>{baseRadius}m</color> " +
                $"<color=#{StackColor}>(+{bonusRadius}m per stack)</color> " +
                $"radius";
        }
    }
}
