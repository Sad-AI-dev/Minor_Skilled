using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core {
    [RequireComponent(
        typeof(AgentAbilities),
        typeof(StatusEffectHandler),
        typeof(AgentHealthManager)
    )]
    public class Agent : MonoBehaviour
    {
        public AgentStats stats;
        [HideInInspector] public AgentAbilities abilities;
        [HideInInspector] public StatusEffectHandler effectHandler;
        [HideInInspector] public AgentHealthManager health;

        private void Start()
        {
            //setup abilities
            abilities = GetComponent<AgentAbilities>();
            abilities.Initialize(this);
            //setup effect handler
            effectHandler = GetComponent<StatusEffectHandler>();
            effectHandler.agent = this;
            //setup health manager
            health = GetComponent<AgentHealthManager>();
            health.agent = this;
        }
    }
}