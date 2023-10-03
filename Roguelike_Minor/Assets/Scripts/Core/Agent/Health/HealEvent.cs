using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core {
    public class HealEvent
    {
        //source data
        public bool hasAgentSource;
        public Agent source;

        //target
        public AgentHealthManager target;

        //heal values
        public readonly float baseHeal;
        public float healMultiplier;

        //ctor
        public HealEvent(float baseHeal, Agent source = null)
        {
            this.source = source;
            hasAgentSource = source != null;
            this.baseHeal = baseHeal;
            //initialize base vars
            healMultiplier = 1;
        }

        public float GetTotalHeal()
        {
            return baseHeal * healMultiplier;
        }
    }
}
