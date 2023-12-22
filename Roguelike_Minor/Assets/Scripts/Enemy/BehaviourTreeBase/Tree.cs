using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using UnityEngine.AI;

namespace Game.Enemy.Core {
    [RequireComponent(typeof(Rigidbody))]
    public abstract class Tree : MonoBehaviour
    {
        public BT_Node root = null;

        [Header("General Variables")]
        public Agent agent;
        public NavMeshAgent navAgent;
        public Rigidbody rb;

        protected virtual void Awake()
        {
            if (rb == null) rb = GetComponent<Rigidbody>();
            if (navAgent != null)
            {
                navAgent.enabled = true;

                NavMeshHit closestHit;

                if (NavMesh.SamplePosition(gameObject.transform.position, out closestHit, 500f, NavMesh.AllAreas))
                    gameObject.transform.position = closestHit.position;
                else
                    Debug.LogError("Could not find position on NavMesh!");
            }
            root = SetupTree();
        }

        protected virtual void Start()
        {
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
            if(null != agent.GetStatsSO().scalingStats) 
            { 
                agent.stats.baseDamage += GameScalingManager.instance.enemyLevel * agent.GetStatsSO().scalingStats.baseDamageScaling;
                agent.stats.Money += Mathf.RoundToInt(GameScalingManager.instance.enemyLevel * agent.GetStatsSO().scalingStats.moneyScaling);
                agent.stats.maxHealth += GameScalingManager.instance.enemyLevel * agent.GetStatsSO().scalingStats.maxHealthScaling;

                HealEvent heal = new HealEvent(agent.stats.maxHealth)
                {
                    createNumLabel = false
                };

                agent.health.Heal(heal);
            }
        }
    }
}
