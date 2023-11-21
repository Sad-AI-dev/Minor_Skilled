using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Core.GameSystems
{
    public static class Explosion
    {        
        public static List<Agent> FindAgentsInRange(Vector3 pos, float radius, Agent source = null)
        {
            Collider[] overlappingColliders = Physics.OverlapSphere(pos, radius);

            List<Agent> agentsInRange = new List<Agent>();

            foreach (Collider collider in overlappingColliders)
            {
                if (collider.TryGetComponent<Agent>(out Agent agent))
                {
                    if (source != null)
                    {
                        if (!collider.CompareTag(source.tag))
                            agentsInRange.Add(agent);
                    }
                    else
                    {
                        agentsInRange.Add(agent);
                    }  
                }
            }

            return agentsInRange;
        }

        public static void DealDamage(List<Agent> agents, Agent source, int damage)
        {
            HitEvent hitEvent = new HitEvent(source);
            hitEvent.baseDamage = damage;

            foreach (Agent agent in agents)
            {
                agent.health.Hurt(hitEvent);
            }
        }

        public static void DealKnockback(List<Agent> agents, float knockbackForce, Vector3 position)
        {
            Vector3 knockbackVelocity;

            foreach (Agent agent in agents)
            {
                Vector3 agentPos = agent.transform.position + new Vector3(0, agent.transform.localScale.y / 2, 0);
                knockbackVelocity = (agentPos - position).normalized;
                knockbackVelocity *= knockbackForce;
                agent.OnKnockbackReceived.Invoke(knockbackVelocity);
            }
        }

        public static void AddStatusEffect(List<Agent> agents, StatusEffectSO effect, int stacks = 1)
        {
            foreach(Agent agent in agents)
            {
                agent.effectHandler.AddEffect(effect, stacks);
            }
        }
    }
}
