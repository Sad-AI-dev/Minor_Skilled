using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core {
    [RequireComponent(typeof(AgentAbilities))]
    [RequireComponent(typeof(StatusEffectHandler))]
    [RequireComponent(typeof(AgentHealthManager))]
    public class Agent : MonoBehaviour
    {
        public AgentStats stats;
        [HideInInspector] public AgentAbilities abilities;
        [HideInInspector] public StatusEffectHandler effectHandler;
        [HideInInspector] public AgentHealthManager health;
        [HideInInspector] public Inventory inventory;

        private void Awake()
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
            //setup inventory
            inventory = GetComponent<Inventory>();
            inventory.agent = this;
        }
    }
}