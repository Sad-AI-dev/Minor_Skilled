using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.GameSystems
{
    [RequireComponent(typeof(SphereCollider))]
    public class DOTExplosion : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] private int ticks;
        [SerializeField] private float tickCooldown;
        [SerializeField] private float radius;
        [SerializeField] private bool includeCaster;
        [SerializeField] private bool onlyAffectGrounded;

        [Header("Damage")]
        [SerializeField] public float damage;

        [Header("Knockback")]
        [SerializeField] private int knockbackForce;

        [Header("StatusEffect")]
        [SerializeField] private StatusEffectSO effect;
        [SerializeField] private int effectStacks;

        [HideInInspector] public Agent source = null;

        private List<Agent> agentsInRange = new List<Agent>();
        private List<Agent> agentsToTarget = new List<Agent>();
        
        private SphereCollider col;

        private void Start()
        {
            col = GetComponent<SphereCollider>();
            col.isTrigger = true;
            transform.localScale *= radius * 2;
            StartCoroutine(WaitForNextFrameCo());
        }

        private void ExecuteTick()
        {
            if(onlyAffectGrounded)
            {
                List<Agent> groundedAgents = new List<Agent>();
                foreach(Agent agent in agentsInRange)
                {
                    if(agent.isGrounded)
                        groundedAgents.Add(agent);
                }

                agentsToTarget = groundedAgents;
            }
            else
            {
                agentsToTarget = agentsInRange;
            }

            if(damage > 0 && source != null)
                Explosion.DealDamage(agentsToTarget, source, damage);
            if(knockbackForce > 0)
                Explosion.DealKnockback(agentsToTarget, knockbackForce, transform.position);
            if(effect != null)
                Explosion.AddStatusEffect(agentsToTarget, effect, effectStacks);
        }

        private IEnumerator WaitForNextFrameCo()
        {
            yield return null;
            
            ExecuteTick();

            StartCoroutine(TickCooldownCo());
        }

        private IEnumerator TickCooldownCo()
        {
            yield return new WaitForSeconds(tickCooldown);
            ticks--;
            if (ticks > 0)
            {
                StartCoroutine(WaitForNextFrameCo());
            }

            else
                Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent<Agent>(out Agent agent))
            {
                if(!includeCaster)
                {
                    if(!agent.CompareTag(source.tag))
                        agentsInRange.Add(agent);
                }
                else
                {
                    agentsInRange.Add(agent);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<Agent>(out Agent agent))
            {
                agentsInRange.Remove(agent);
            }
        }
    }
}
