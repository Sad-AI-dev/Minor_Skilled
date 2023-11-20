using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Core.GameSystems;

namespace Game {
    [CreateAssetMenu(fileName = "17Explosive_Boots", menuName = "ScriptableObjects/Items/T2/17: Explosive Boots", order = 217)]
    public class Item17SO : ItemDataSO
    {
        private class Item17Vars : Item.ItemVars
        {
            public float damageMult;
            public float range;
        }

        [Header("Damage Settings")]
        public float baseDamage = 1f;
        public float bonusDamage = 1f;

        [Header("Range Settings")]
        public float baseRange = 3f;
        public float bonusRange = 2f;

        [Header("Explosion Settings")]
        public GameObject visualsPrefab;

        //========= Initialize Vars ============
        public override void InitializeVars(Item item)
        {
            item.vars = new Item17Vars();
        }

        //========= Manage Stacks ===========
        public override void AddStack(Item item)
        {
            Item17Vars vars = item.vars as Item17Vars;
            if (item.stacks == 1)
            {
                vars.damageMult += baseDamage;
                vars.range += baseRange;
                item.agent.abilities.utility.onUse.AddListener(OnUseUtility);
            }
            else
            {
                vars.damageMult += bonusDamage;
                vars.range += bonusRange;
            }
        }

        public override void RemoveStack(Item item)
        {
            Item17Vars vars = item.vars as Item17Vars;
            if (item.stacks == 1)
            {
                vars.damageMult -= baseDamage;
                vars.range -= baseRange;
                item.agent.abilities.utility.onUse.RemoveListener(OnUseUtility);
            }
            else
            {
                vars.damageMult -= bonusDamage;
                vars.range -= bonusRange;
            }
        }

        //========= Handle Utility Use =========
        private void OnUseUtility(Ability ability)
        {
            Item17Vars vars = ability.agent.inventory.GetItemOfType(this).vars as Item17Vars;
            Vector3 pos = ability.agent.transform.position;
            //create explosion
            List<Agent> agentsInRange = Explosion.FindAgentsInRange(pos, vars.range, ability.agent);
            //deal damage to agents
            HitEvent hitEvent = new HitEvent(ability.agent, 1f);
            hitEvent.baseDamage = ability.agent.stats.baseDamage * vars.damageMult;
            foreach (Agent agent in agentsInRange)
            {
                agent.health.Hurt(hitEvent);
            }
            //create visuals
            GameObject visuals = Instantiate(visualsPrefab);
            visuals.transform.position = pos;
            visuals.transform.localScale = Vector3.one * (vars.range * 2);
        }

        //========== Description ===========
        public override string GenerateLongDescription()
        {
            return $"using your Utility ability creates a " +
                $"<color=#{HighlightColor}>{baseRange}m</color> " +
                $"<color=#{StackColor}>(+{bonusRange}m per stack)</color> " +
                $"explosion, dealing" +
                $" <color=#{HighlightColor}>{baseDamage * 100}%</color> " +
                $"<color=#{StackColor}>(+{bonusDamage * 100}% per stack)</color> damage";
        }
    }
}
