using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Enemy.Core;
using Game.Core;
using Game.Core.GameSystems;

namespace Game.Enemy
{
    [CreateAssetMenu(fileName = "SmallSquidPrimary", menuName = "ScriptableObjects/Enemy/SmallSquid/Primary")]
    public class SmallSquidPrimarySO : AbilitySO
    {
        public float KnockbackForce = 2;

        public override void InitializeVars(Ability source)
        {
            
        }

        public override void Use(Ability source)
        {
            Agent agent = source.agent;
            List<Agent> agents = Explosion.FindAgentsInRange(agent.transform.position, SmallSquidTree.ExplosionRange, agent);
            if (agents.Count > 0)
            {
                Debug.Log("Hit " + agents[0].name);
                Explosion.DealDamage(agents, agent, Mathf.RoundToInt(agent.stats.baseDamage * source.abilityData.damageMultiplier));
                Explosion.DealKnockback(agents, KnockbackForce, agent.transform.position);
            }
            Destroy(agent.gameObject);
        }
    }
}
