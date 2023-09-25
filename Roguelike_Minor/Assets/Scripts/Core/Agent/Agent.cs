using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core {
    [RequireComponent(typeof(AgentStats), typeof(AgentAbilities))]
    public class Agent : MonoBehaviour
    {
        public AgentStats stats;
        public AgentAbilities abilities;

        private void Start()
        {
            stats = GetComponent<AgentStats>();
            abilities = GetComponent<AgentAbilities>();
            abilities.Initialize(this);
        }
    }
}