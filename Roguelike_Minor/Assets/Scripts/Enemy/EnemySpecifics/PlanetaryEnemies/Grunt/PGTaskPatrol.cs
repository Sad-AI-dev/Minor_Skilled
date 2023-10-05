using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Enemy.Core;
using UnityEngine.AI;

namespace Game.Enemy
{
    public class PGTaskPatrol : BT_Node
    {
        Transform transform;
        NavMeshAgent agent;

        bool waiting = false;
        float waitTime = 2;
        float currentTime = 0;
        Vector3 moveToPosition;
        float RandomPatrolRange = 10;

        public PGTaskPatrol(Transform transform, NavMeshAgent agent)
        {
            this.transform = transform;
            this.agent = agent;

            agent.speed = PGTree.speed;
            moveToPosition = RandomNavmeshLocation(5);
        }

        public override NodeState Evaluate()
        {
            // Waiting for patrol
            if (waiting)
            {
                currentTime += Time.deltaTime;

                if (currentTime >= waitTime)
                {
                    //Get new random position to move to
                    moveToPosition = RandomNavmeshLocation(RandomPatrolRange);

                    if (moveToPosition != null)
                    {
                        waiting = false;
                    }
                }
            }
            else
            {
                //Move to new position;
                if(GetData("Target") == null) agent.SetDestination(moveToPosition);

                //Check if arived at target
                if (!agent.pathPending)
                {
                    if (agent.remainingDistance <= agent.stoppingDistance)
                    {
                        if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                        {
                            waiting = true;
                            currentTime = 0;
                        }
                    }
                }
            }

            state = NodeState.RUNNING;
            return state;
        }

        public Vector3 RandomNavmeshLocation(float radius)
        {
            Vector3 randomDirection = Random.insideUnitSphere * radius;
            randomDirection += transform.position;
            NavMeshHit hit;
            Vector3 finalPosition = Vector3.zero;
            if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
            {
                finalPosition = hit.position;
            }
            return finalPosition;
        }
    }
}
