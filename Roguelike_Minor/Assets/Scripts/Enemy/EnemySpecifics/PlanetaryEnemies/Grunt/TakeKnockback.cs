using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Enemy.Core;
using Game.Core;
using UnityEngine.AI;
using System.Threading.Tasks;

namespace Game.Enemy {
    public class TakeKnockback : BT_Node
    {
        private Transform transform;
        private NavMeshAgent agent;
        private Agent enemyAgent;
        private Rigidbody rb;

        //Bools
        private bool takingKnockback = false;
        private bool coStarted = false;
        private bool grounded = true;

        //Multipliers
        private Vector3 knockbackDirection;
        private float upMultiplier = 1;
        private float directionMultiplier = 6;

        public TakeKnockback(Transform transform, Agent enemyAgent, NavMeshAgent agent, Rigidbody rb, float upMultiplier, float directionMultiplier)
        {
            this.transform = transform;
            this.enemyAgent = enemyAgent;
            this.agent = agent;
            this.rb = rb;
            this.upMultiplier = upMultiplier;
            this.directionMultiplier = directionMultiplier;

            enemyAgent.OnKnockbackReceived.AddListener(OnKnockbackRecieved);
        }

        void OnKnockbackRecieved(Vector3 dir)
        {
            Debug.Log("knockback Recieved");
            takingKnockback = true;
            knockbackDirection = dir;
        }

        public override NodeState Evaluate()
        {
            if (takingKnockback)
            {
                grounded = Physics.Raycast(transform.position, -transform.up, 1f);
                if (!coStarted)
                {
                    //Nav mesh knockback
                    //agent.velocity = knockbackDirection * 8;
                    //KnockbackNavmesh();

                    //RB Knockback
                    rb.isKinematic = false;
                    rb.velocity = (knockbackDirection + (Vector3.up * upMultiplier)) * directionMultiplier;
                    KnockbackRB();
                }
                state = NodeState.RUNNING;
            }
            else
            {
                state = NodeState.FAILURE;
            }

            return state;
        }

        private async void KnockbackNavmesh()
        {
            coStarted = true;
            agent.speed = 10;
            agent.angularSpeed = 0;
            agent.acceleration = 20;

            await Task.Delay(200);

            agent.speed = enemyAgent.stats.walkSpeed;
            agent.angularSpeed = 180;
            agent.acceleration = 10;
            coStarted = false;
            takingKnockback = false;
        }
        private async void KnockbackRB()
        {
            coStarted = true;
            rb.useGravity = true;
            agent.enabled = false;
            grounded = false;

            await Task.Delay(200);

            while (!grounded)
            {
                await Task.Delay(1000);

                if (grounded)
                {
                    Debug.Log("Hit ground");
                    agent.enabled = true;
                    rb.isKinematic = true;
                    rb.useGravity = false;

                    coStarted = false;
                    takingKnockback = false;
                }
            }
            
            
        }
    }
}
