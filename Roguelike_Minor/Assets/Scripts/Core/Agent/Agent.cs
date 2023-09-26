using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core {
    [RequireComponent(typeof(AgentAbilities), typeof(StatusEffectHandler))]
    public class Agent : MonoBehaviour
    {
        public AgentStats stats;
        [HideInInspector] public AgentAbilities abilities;
        [HideInInspector] public StatusEffectHandler effectHandler;

        private void Start()
        {
            stats = GetComponent<AgentStats>();
            //setup abilities
            abilities = GetComponent<AgentAbilities>();
            abilities.Initialize(this);
            //setup effect handler
            effectHandler = GetComponent<StatusEffectHandler>();
            effectHandler.agent = this;
        }
    }
}