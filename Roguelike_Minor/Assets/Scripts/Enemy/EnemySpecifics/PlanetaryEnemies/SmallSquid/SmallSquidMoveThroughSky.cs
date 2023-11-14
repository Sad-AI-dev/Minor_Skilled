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

        Vector3 randomWaipoint = Vector3.zero;

        public SmallSquidMoveThroughSky(Transform transform, Agent agent)
        {
            this.transform = transform;
            this.agent = agent;
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
            //Get direction of the point
            Vector3 Direction = (randomWaipoint - transform.position).normalized;

            //Get distance to that point
            float distance = Vector3.Distance(transform.position, randomWaipoint);

            //Check if path is valid
            RaycastHit hit;
            if(Physics.SphereCast(transform.position, 0.5f, Direction, out hit, distance))
            {
                state = NodeState.FAILURE;
                return state;
            }

            //If point close enough, set point to zero
            if (distance <= 0.1f)
            {
                randomWaipoint = Vector3.zero ;
            }

            //Handle move if not at point yet
            if(randomWaipoint != Vector3.zero)
            {
                transform.Translate(Direction * (agent.stats.walkSpeed * Time.deltaTime));
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
