using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Core.GameSystems;

using EffectVars = Game.VulnerableEffectSO.VulnerableVars;

namespace Game {
    [RequireComponent(typeof(Rigidbody))]
    public class SoulFragmentProjectile : MonoBehaviour
    {
        //vars set by item 20
        [HideInInspector] public Item20SO settings; //take settings from here
        [HideInInspector] public Item owner;
        [HideInInspector] public HitEvent sourceEvent;
        [HideInInspector] public float damageMult;
        //target
        [HideInInspector] public Vector3 targetPos;

        //refs
        private Rigidbody rb;
        //shorthand
        private float Speed { get { return settings.moveSpeed * Time.fixedDeltaTime * 100f; } }

        //vars
        private Coroutine explodeRoutine;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            rb.velocity = GetTargetVelocity();
            if (explodeRoutine != null) { DistanceCheck(); }
        }

        private Vector3 GetTargetVelocity()
        {
            return (targetPos - transform.position).normalized * Speed;
        }

        //======= Distance Check =======
        private void DistanceCheck()
        {
            if (Vector3.Distance(transform.position, targetPos) < rb.velocity.magnitude)
            {
                explodeRoutine = StartCoroutine(DelayExplodeCo());
            }
        }

        private IEnumerator DelayExplodeCo()
        {
            yield return new WaitForSeconds(0.1f);
            ApplyVulnerable();
            gameObject.SetActive(false);
        }

        //======= Hit Detection =========
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.Equals(owner.agent.gameObject)) { return; }
            if (other.TryGetComponent(out Agent agent)) {
                StopCoroutine(DelayExplodeCo());
                //deal damage to agent
                DealDamage(agent);
                //apply vulnerabel
                ApplyVulnerable();
            }
            //setup for reuse
            gameObject.SetActive(false);
        }

        private void DealDamage(Agent target)
        {
            //setup hit event
            HitEvent hit = new HitEvent(sourceEvent, owner, settings.procCoef);
            hit.baseDamage = owner.agent.stats.baseDamage * damageMult;
            //deal damage
            target.health.Hurt(hit);
        }

        //========== Vulnerable =========
        private void ApplyVulnerable()
        {
            //find agents in range
            List<Agent> agents = Explosion.FindAgentsInRange(transform.position, settings.effectRange, owner.agent);
            //apply vulnerable to each agent
            foreach (Agent agent in agents)
            {
                EffectVars effectVars = agent.effectHandler.AddEffect(settings.vulnerableEffect) as EffectVars;
                effectVars.damageMult = settings.vulnerableDamageMult;
            }
        }
    }
}
