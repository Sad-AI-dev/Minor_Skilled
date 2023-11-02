using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Enemy.Core;
using Game.Core;
using UnityEngine.AI;

namespace Game.Enemy
{
    public class PGTaskAttackPlayerMelee : BT_Node
    {
        private Transform transform;
        private NavMeshAgent agent;
        private Agent enemyAgent;
        private float distanceToTarget;

        public PGTaskAttackPlayerMelee(Transform transform, Agent enemyAgent, NavMeshAgent agent)
        {
            this.transform = transform;
            this.enemyAgent = enemyAgent;
            this.agent = agent;
        }

        public override NodeState Evaluate()
        {
            //Get data
            Transform target = (Transform)GetData("Target");
            if (GetData("DistanceToTarget") != null) 
            {
                distanceToTarget = (float)GetData("DistanceToTarget");
            }

            //Check if there is a target;
            if(target == null)
            {
                state = NodeState.FAILURE;
            } else if (distanceToTarget <= PGTree.meleeAttackRange) //Check if we are close enough for melee attack
            {
                //Stop moving
                agent.velocity = Vector3.zero;

                //Rotate to target
                Vector3 targetPostition = new Vector3(target.position.x, transform.position.y, target.position.z);
                transform.LookAt(targetPostition);

                //Use ability
                enemyAgent.abilities.primary.TryUse();
                state = NodeState.RUNNING;
            }
            else //fail if nothing applies.
            {
                state = NodeState.FAILURE;
            }

            return state;
        }
    }
}
