using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Core {
    public class HitEvent
    {
        //source data
        public bool hasAgentSource;
        public Agent source;

        //target
        public AgentHealthManager target;

        //damage values
        public float baseDamage;
        public float damageMultiplier;
        public float damageReduction;

        //death event
        public UnityEvent<Agent> onDeath;

        //ctor
        public HitEvent(Ability source = null)
        {
            hasAgentSource = source != null;

            if (hasAgentSource)
            {
                this.source = source.agent;
                //setup base damage
                SetupBaseDamage(source);
            }

            InitializeVars();
        }
        //alternate ctor (for additional projectiles from items for example)
        public HitEvent(Agent source)
        {
            this.source = source;
            hasAgentSource = source != null;
            InitializeVars();
        }

        private void SetupBaseDamage(Ability source)
        {
            baseDamage = source.agent.stats.baseDamage + source.abilityData.damageMultiplier;
        }
        private void InitializeVars()
        {
            damageMultiplier = 1f;
            onDeath = new UnityEvent<Agent>();
        }

        //============== Get Total Damage ===============
        public float GetTotalDamage()
        {
            return (baseDamage * damageMultiplier) - damageReduction;
        }
    }
}
