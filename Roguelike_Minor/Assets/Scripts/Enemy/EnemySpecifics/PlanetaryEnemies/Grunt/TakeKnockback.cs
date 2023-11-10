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
        private NavMeshAgent navAgent;
        private Agent agent;
        private Rigidbody rb;

        //Bools
        private bool takingKnockback = false;
        private bool coStarted = false;
        private bool grounded = true;

        //Multipliers
        private Vector3 knockbackDirection;
        private float upMultiplier = 1;
        private float directionMultiplier = 6;

        public TakeKnockback(Transform transform, Agent agent, NavMeshAgent navAgent, Rigidbody rb, float upMultiplier, float directionMultiplier)
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
            //Debug.Log("Taking Knockback enemy");
            takingKnockback = true;
            knockbackDirection = dir;
        }

        public override NodeState Evaluate()
        {
            if (takingKnockback)
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

            return state;
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
                    //Debug.Log("Hit ground");
                    navAgent.enabled = true;
                    rb.isKinematic = true;
                    rb.useGravity = false;

                    coStarted = false;
                    takingKnockback = false;
                }
            }
            
            
        }
    }
}
