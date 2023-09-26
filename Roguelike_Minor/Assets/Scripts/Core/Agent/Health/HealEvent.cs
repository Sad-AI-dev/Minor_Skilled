using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core {
    public class HealEvent
    {
        //source data
        public bool hasAgentSource;
        public Ability source;

        //target
        public AgentHealthManager target;

        //heal values
        public float baseHeal;
        public float healMultiplier;

        //ctor
        public HealEvent(Ability source)
        {
            this.source = source;
            hasAgentSource = source != null;

            if (hasAgentSource)
            {
                //TODO figure out how to set baseHeal here lol
            }
        }
    }
}
