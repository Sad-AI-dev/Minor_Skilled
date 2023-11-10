using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using UnityEngine.AI;

namespace Game.Enemy.Core
{
    public abstract class Tree : MonoBehaviour
    {
        private BT_Node root = null;

        [Header("General Variables")]
        public Agent agent;
        public NavMeshAgent navAgent;

        [Header("Scaling Variables")]
        public float baseDamageScaling;
        public int moneyScaling;
        public float maxHealthScaling;

        protected virtual void Start()
        {
            root = SetupTree();
            HandleScaling();

            if(agent && !agent.enabled)
            {
                agent.enabled = true;
            }
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
            agent.stats.Money += GameScalingManager.instance.enemyLevel * moneyScaling;
            agent.stats.maxHealth += GameScalingManager.instance.enemyLevel * maxHealthScaling;

            HealEvent heal = new HealEvent(agent.stats.maxHealth)
            {
                createNumLabel = false
            };

            agent.health.Heal(heal);
        }
    }
}
