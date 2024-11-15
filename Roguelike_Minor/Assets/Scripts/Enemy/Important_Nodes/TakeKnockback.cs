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
        private Rigidbody rb;

        //Bools
        private bool coStarted = false;
        private bool grounded = true;

        //Multipliers
        private Vector3 knockbackDirection;
        private float upMultiplier = 1;
        private float directionMultiplier = 6;

        public TakeKnockback(Transform transform, Agent agent, Rigidbody rb, float upMultiplier, float directionMultiplier, NavMeshAgent navAgent = null)
        {
            this.transform = transform;
            this.agent = agent;
            this.navAgent = navAgent;
            this.rb = rb;
            this.upMultiplier = upMultiplier;
            this.directionMultiplier = directionMultiplier;

            agent.OnKnockbackReceived.AddListener(OnKnockbackRecieved);
        }

        void OnKnockbackRecieved(Vector3 dir)
        {
            SetData("TakingKnockback", true);
            knockbackDirection = dir;
        }

        public override NodeState Evaluate()
        {
            // Knockback for Navmesh Agent
            if (navAgent != null)
            {
                if (GetData("TakingKnockback") != null && (bool)GetData("TakingKnockback"))
                {
                    grounded = Physics.Raycast(transform.position, -transform.up, 0.2f);
                    if (!coStarted)
                    {
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
            }
            // Knockback for Flying Enemy
            else
            {
                if (GetData("TakingKnockback") != null && (bool)GetData("TakingKnockback"))
                {
                    rb.velocity = (knockbackDirection + (Vector3.up * upMultiplier)) * directionMultiplier;
                    agent.StartCoroutine(HandleFlyingKnockback());

                    state = NodeState.RUNNING;
                }
                else
                {
                    state = NodeState.FAILURE;
                }
            }

            return state;
        }

        IEnumerator HandleFlyingKnockback()
        {
            yield return new WaitForSeconds(0.2f);
            SetData("TakingKnockback", false);
        }

        private async void KnockbackRB()
        {
            coStarted = true;
            rb.useGravity = true;
            navAgent.enabled = false;
            grounded = false;

            await Task.Delay(200);

            while (!grounded)
            {
                await Task.Delay(1000);

                if (grounded)
                {
                    navAgent.enabled = true;
                    rb.isKinematic = true;
                    rb.useGravity = false;

                    coStarted = false;
                    SetData("TakingKnockback", false);
                }
            }
        }
    }
}
