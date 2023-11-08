using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Core.GameSystems
{
    public class Explosion
    {        
        public List<Agent> FindAgentsInRange(Vector3 pos, float radius, bool includePlayer = false)
        {
            Collider[] overlappingColliders = Physics.OverlapSphere(pos, radius);

            List<Agent> agentsInRange = new List<Agent>();

            foreach(Collider collider in overlappingColliders)
            {
                if(collider.TryGetComponent<Agent>(out Agent agent))
                {
                    if(!collider.CompareTag("Player") || includePlayer)
                        agentsInRange.Add(agent);
                }
            }

            return agentsInRange;
        }

        public void DealDamage(Agent source, int damage, List<Agent> agents)
        {
            HitEvent hitEvent = new HitEvent(source);
            //hitEvent.baseDamage = damage;

            foreach (Agent agent in agents)
            {
                agent.health.Hurt(hitEvent);
            }
        }

        public void DealKnockback(float knockbackForce, Vector3 position, List<Agent> agents)
        {
            Vector3 knockbackVelocity;

            foreach (Agent agent in agents)
            {
                Vector3 agentPos = agent.transform.position + new Vector3(0, agent.transform.localScale.y / 2, 0);
                Debug.Log(position + ", " + agentPos);
                knockbackVelocity = (agentPos - position).normalized;
                knockbackVelocity *= knockbackForce;
                agent.OnKnockbackReceived.Invoke(knockbackVelocity);
            }
        }
    }
}
