using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core {
    public class AgentAbilities : MonoBehaviour
    {
        [HideInInspector] public Agent agent;

        public Ability primary;
        public Ability secondary;
        public Ability special;
        public Ability utility;

        private void Start()
        {
            ResetAbilities(); //reset all cooldowns on level load
        }

        public void Initialize(Agent agent)
        {
            this.agent = agent;
            //initialize abilities
            primary.Initialize(agent);
            secondary.Initialize(agent);
            special.Initialize(agent);
            utility.Initialize(agent);
        }

        //============= Reset all abilities =================
        public void ResetAbilities()
        {
            primary.Reset();
            secondary.Reset();
            special.Reset();
            utility.Reset();
        }
    }
}
