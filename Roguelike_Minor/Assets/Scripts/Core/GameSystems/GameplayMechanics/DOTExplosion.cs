using PlasticPipe.PlasticProtocol.Messages;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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

        [Header("Damage")]
        [SerializeField] private int damage;

        [Header("Knockback")]
        [SerializeField] private int knockbackForce;

        [Header("StatusEffect")]
        [SerializeField] private StatusEffectSO effect;
        [SerializeField] private int effectStacks;

        [HideInInspector] public Agent source = null;

        private bool canTick = true;

        private List<Agent> agentsInRange = new List<Agent>();
        private Explosion explosion = new Explosion();
        
        private SphereCollider col;

        private void Start()
        {
            col = GetComponent<SphereCollider>();
            col.isTrigger = true;
            transform.localScale *= radius * 2;

            if(canTick)
                StartCoroutine(WaitForNextFrameCo());
        }

        private void ExecuteTick()
        {
            if(damage > 0 && source != null)
                explosion.DealDamage(agentsInRange, source, damage);
            if(knockbackForce > 0)
                explosion.DealKnockback(agentsInRange, knockbackForce, transform.position);
            if(effect != null)
                explosion.AddStatusEffect(agentsInRange, effect, effectStacks);

            canTick = false;
        }

        private IEnumerator WaitForNextFrameCo()
        {
            yield return null;
            
            ExecuteTick();

            if(!canTick)
                StartCoroutine(TickCooldownCo());
        }

        private IEnumerator TickCooldownCo()
        {
            yield return new WaitForSeconds(tickCooldown);
            ticks--;
            if (ticks > 0)
            {
                Debug.Log(tickCooldown);
                StartCoroutine(WaitForNextFrameCo());
            }

            else
                Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent<Agent>(out Agent agent))
            {
                if(source != null)
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