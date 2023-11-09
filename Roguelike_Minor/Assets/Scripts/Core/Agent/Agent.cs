using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Core {

    [System.Serializable]
    public class KnockbackEvent : UnityEvent<Vector3>
    {
    }

    [RequireComponent(typeof(AgentAbilities))]
    [RequireComponent(typeof(StatusEffectHandler))]
    [RequireComponent(typeof(AgentHealthManager))]
    public class Agent : MonoBehaviour
    {
        [SerializeField] private AgentStatsSO baseStats;
        [HideInInspector] public AgentStats stats;
        [HideInInspector] public AgentAbilities abilities;
        [HideInInspector] public StatusEffectHandler effectHandler;
        [HideInInspector] public AgentHealthManager health;
        [HideInInspector] public Inventory inventory;
        [HideInInspector] public KnockbackEvent OnKnockbackReceived;

        [HideInInspector] public bool isGrounded;

        private void Awake()
        {
            //setup stats
            stats.Copy(baseStats.baseStats);
            //setup abilities
            abilities = GetComponent<AgentAbilities>();
            abilities.agent = this;
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