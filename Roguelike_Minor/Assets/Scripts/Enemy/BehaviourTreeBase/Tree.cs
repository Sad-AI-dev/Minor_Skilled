using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using UnityEngine.AI;

namespace Game.Enemy.Core {
    [RequireComponent(typeof(Rigidbody))]
    public abstract class Tree : MonoBehaviour
    {
        private BT_Node root = null;

        [Header("General Variables")]
        public Agent agent;
        public NavMeshAgent navAgent;
        public Rigidbody rb;

        [Header("Scaling Variables")]
        public float baseDamageScaling = 1;
        public float moneyScaling = 1;
        public float maxHealthScaling = 1;

        protected virtual void Awake()
        {
            if (rb == null) rb = GetComponent<Rigidbody>();
            if (navAgent != null)
            {
                navAgent.enabled = true;
            }
        }

        protected virtual void Start()
        {
            root = SetupTree();
            HandleScaling();
        }

        protected virtual void Update()
        {
            if(root != null)
            {
                root.Evaluate();
            }

            if(navAgent != null && agent != null)
            {
                agent.isGrounded = navAgent.enabled;
            }
        }
        protected abstract BT_Node SetupTree();
    
        void HandleScaling()
        {
            //SETUP VARIABLE SCALING
            agent.stats.baseDamage += GameScalingManager.instance.enemyLevel * baseDamageScaling;
            agent.stats.Money += Mathf.RoundToInt(GameScalingManager.instance.enemyLevel * moneyScaling);
            agent.stats.maxHealth += GameScalingManager.instance.enemyLevel * maxHealthScaling;

            HealEvent heal = new HealEvent(agent.stats.maxHealth)
            {
                createNumLabel = false
            };

            agent.health.Heal(heal);
        }
    }
}
