using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Game.Core;
using Game.Core.GameSystems;

using EffectVars = Game.VulnerableEffectSO.VulnerableVars;

namespace Game {
    [RequireComponent(typeof(Rigidbody))]
    public class SoulFragmentProjectile : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private TrailRenderer trail;

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
        private float Speed { get { return settings.moveSpeed * Time.fixedDeltaTime; } }

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            //move
            rb.MovePosition(Vector3.MoveTowards(transform.position, targetPos, Speed));
            //distance check
            DistanceCheck();
        }

        //======= Distance Check =======
        private void DistanceCheck()
        {
            if (Vector3.Distance(transform.position, targetPos) < 0.1f)
            {
                //reached destination, explode
                Explode();
            }
        }

        //======= Hit Detection =========
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.Equals(owner.agent.gameObject)) { return; }
            //explode on impact
            Explode();
        }

        private void Explode()
        {
            //deal damage to agent
            DealDamage();
            //apply vulnerabel
            ApplyVulnerable();
            //setup for reuse
            gameObject.SetActive(false);
        }

        private void DealDamage()
        {
            List<Agent> agents = Explosion.FindAgentsInRange(transform.position, settings.damageRange, owner.agent);
            //damage agents in range
            foreach (Agent agent in agents)
            {
                //setup hit event
                HitEvent hit = new HitEvent(sourceEvent, owner, settings.procCoef) {
                    baseDamage = owner.agent.stats.baseDamage * damageMult
                };
                //deal damage
                agent.health.Hurt(hit);
            }
        }

        //========== Vulnerable =========
        private void ApplyVulnerable()
        {
            //find agents in range
            List<Agent> agents = Explosion.FindAgentsInRange(transform.position, settings.effectRange, owner.agent);
            //apply vulnerable to each agent
            foreach (Agent agent in agents)
            {
                if (TargetIsValid(agent))
                {
                    EffectVars effectVars = agent.effectHandler.AddEffect(settings.vulnerableEffect) as EffectVars;
                    effectVars.source = settings;
                    effectVars.damageMult = settings.vulnerableDamageMult;
                }
            }
        }

        private bool TargetIsValid(Agent target)
        {
            if (target.effectHandler.statusEffects.ContainsKey(settings.vulnerableEffect))
            {
                foreach (EffectVars vars in target.effectHandler.statusEffects[settings.vulnerableEffect].Cast<EffectVars>())
                {
                    if (vars.source == settings) { return false; }
                }
            }
            return true;
        }

        //===== Setup Trail ======
        public void SetupTrail()
        {
            trail.Clear();
        }
    }
}
