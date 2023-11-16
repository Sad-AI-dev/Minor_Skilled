using System;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Core.GameSystems;

namespace Game {
    [CreateAssetMenu(fileName = "21Poisonous_Skull", menuName = "ScriptableObjects/Items/T1/21: Poisonous Skull", order = 121)]
    public class Item21 : ItemDataSO
    {
        private class Item21Vars : Item.ItemVars
        {
            public float range;
        }

        [Header("Cloud settings")]
        public float baseRange = 3f;
        public float bonusRange = 2f;

        [Header("Effect settings")]
        public DOTEffect poisonEffect;
        public float chance = 20f;
        public float poisonDamageMult = 0.5f;

        [Header("Visuals")]
        public GameObject poisonCloudPrefab;

        public override void InitializeVars(Item item)
        {
            item.vars = new Item21Vars();
        }

        //========= Manage Stacks ===========
        public override void AddStack(Item item)
        {
            Item21Vars vars = item.vars as Item21Vars;
            if (item.stacks == 1) { vars.range += baseRange; }
            else { vars.range += bonusRange; }
        }

        public override void RemoveStack(Item item)
        {
            Item21Vars vars = item.vars as Item21Vars;
            if (item.stacks == 0) { vars.range -= baseRange; }
            else { vars.range -= bonusRange; }
        }

        //=========== Process Hit Event ==========
        public override void ProcessDealDamage(ref HitEvent hitEvent, Item item)
        {
            ValueTuple<HitEvent, Item21Vars> tuple = new(hitEvent, item.vars as Item21Vars);
            AgentRandom.TryProc(chance, hitEvent, ApplyPoison, tuple);
        }

        private void ApplyPoison(ValueTuple<HitEvent, Item21Vars> tuple)
        {
            HitEvent hitEvent = tuple.Item1;
            Item21Vars vars = tuple.Item2;
            //apply poison
            List<Agent> agentsInRange = Explosion.FindAgentsInRange(
                hitEvent.target.agent.transform.position,
                vars.range,
                hitEvent.source
            );
            //initialize poison effect
            foreach (Agent agent in agentsInRange)
            {
                agent.effectHandler.AddEffect(poisonEffect);
                //initialize vars
                int varsIndex = agent.effectHandler.statusEffects[poisonEffect].Count - 1;
                DOTEffect.DOTEffectVars effectVars = agent.effectHandler.statusEffects[poisonEffect][varsIndex] as DOTEffect.DOTEffectVars;
                //set values
                effectVars.dmg = poisonDamageMult * hitEvent.source.stats.baseDamage;
                effectVars.source = hitEvent.source;
            }
            //create visual
            GameObject visual = Instantiate(poisonCloudPrefab);
            visual.transform.position = hitEvent.target.transform.position;
            visual.transform.localScale = Vector3.one * (vars.range * 2);
        }

        //========== Description ===========
        public override string GenerateLongDescription()
        {
            return $"Gain a <color=#{HighlightColor}>{chance}%</color> " +
                $"chance to create a <color=#{HighlightColor}>poison cloud</color> on hit in a " +
                $"<color=#{HighlightColor}>{baseRange}m radius</color> " +
                $"<color=#{StackColor}>(+{bonusRange}m per stack)</color>, dealing " +
                $"<color=#{HighlightColor}>{poisonEffect.GetTotalTicks() * poisonDamageMult * 100}% " +
                $"base damage</color> over " +
                $"<color=#{HighlightColor}>{poisonEffect.duration} seconds</color>";
        }
    }
}
