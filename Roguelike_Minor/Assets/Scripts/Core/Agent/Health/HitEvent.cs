using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Core {
    public class HitEvent
    {
        //source data
        public bool hasAgentSource;
        public Ability source;

        //target
        public AgentHealthManager target;

        //damage values
        public readonly float baseDamage;
        public float damageMultiplier;
        public float damageReduction;

        //death event
        public UnityEvent<Agent> onDeath;

        //ctor
        public HitEvent(Ability source)
        {
            this.source = source;
            hasAgentSource = source != null;

            if (hasAgentSource)
            {
                //setup base damage
                baseDamage = source.agent.stats.baseDamage + source.abilityData.damageMultiplier;
            }

            //initialize other vars
            damageMultiplier = 1f;
            onDeath = new UnityEvent<Agent>();
        }
    }
}
