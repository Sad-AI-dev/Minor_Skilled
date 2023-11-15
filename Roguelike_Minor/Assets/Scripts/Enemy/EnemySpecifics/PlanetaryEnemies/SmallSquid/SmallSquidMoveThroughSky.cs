using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Enemy.Core;
using Game.Core;

namespace Game.Enemy {
    public class SmallSquidMoveThroughSky : BT_Node
    {
        Transform transform;
        Agent agent;
        Rigidbody rb;

        Vector3 randomWaipoint = Vector3.zero;
        Vector3 dir = Vector3.zero;

        public SmallSquidMoveThroughSky(Transform transform, Agent agent, Rigidbody rb)
        {
            this.transform = transform;
            this.agent = agent;
            this.rb = rb;
        }

        public override NodeState Evaluate()
        {
            if(GetData("FlightSpaceOrigin") == null)
            {
                SetupFlightSpace();
            }

            //Get random point
            if (randomWaipoint == Vector3.zero)
            {
                randomWaipoint = (Vector3)GetData("FlightSpaceOrigin") + (Random.insideUnitSphere * SmallSquidTree.FlightPatrolRange);
            }

            if (randomWaipoint != Vector3.zero)
            {
                if (GetData("Chasing") != null && (bool)GetData("Chasing") == true)
                {
                    return NodeState.FAILURE;
                }
                else
                {
                    dir = (randomWaipoint - transform.position).normalized;

                    //Handle rotation
                    Quaternion targetRotation = Quaternion.LookRotation(dir);
                    targetRotation = Quaternion.RotateTowards(
                    transform.rotation,
                    targetRotation,
                    360 * Time.deltaTime);

                    //Move
                    //transform.Translate(Direction * (agent.stats.walkSpeed * Time.deltaTime));
                    rb.MovePosition(transform.position + dir * (agent.stats.sprintSpeed * Time.deltaTime));
                    rb.MoveRotation(targetRotation);
                }
            }

            //Get distance to that point
            float distance = Vector3.Distance(transform.position, randomWaipoint);

            //Check if path is valid
            RaycastHit hit;
            if(Physics.SphereCast(transform.position, 0.5f, dir, out hit, distance))
            {
                state = NodeState.FAILURE;
                randomWaipoint = Vector3.zero;
                return state;
            }

            //If point close enough, set point to zero
            if (distance <= 0.1f)
            {
                randomWaipoint = Vector3.zero ;
            }

            state = NodeState.RUNNING;
            return state; 
        }

        void SetupFlightSpace()
        {
            Vector3 pos = transform.position;
            parent.SetData("FlightSpaceOrigin", pos);
        }
    }
}
