using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Enemy.Core;
using Game.Core;
using UnityEngine.AI;

namespace Game.Enemy {
    public class PGTaskAttackPlayerRanged : BT_Node
    {
        private Transform transform;
        private Transform target;

        private NavMeshAgent agent;
        private Agent enemyAgent;

        private float distanceToTarget;

        public PGTaskAttackPlayerRanged(Transform transform, Agent enemyAgent, NavMeshAgent agent)
        {
            this.transform = transform;
            this.enemyAgent = enemyAgent;
            this.agent = agent;
        }

        public override NodeState Evaluate()
        {
            target = (Transform)GetData("Target");
            if (GetData("DistanceToTarget") != null) distanceToTarget = (float)GetData("DistanceToTarget");
            
            //If target null: Fail
            if (target == null)
            {
                state = NodeState.FAILURE;
            }
            //If in melee ranged: Fail
            else if (PGTree.EnemiesInRangeOfPlayer < 3 || distanceToTarget <= PGTree.meleeAttackRange)
            {
                state = NodeState.FAILURE;
            }
            //else Running
            else if(distanceToTarget <= PGTree.rangedAttackRange)
            {
                //Stop moving
                agent.velocity = Vector3.zero;

                //Rotate to target
                Vector3 targetPostition = new Vector3(target.position.x, transform.position.y, target.position.z);
                transform.LookAt(targetPostition);

                enemyAgent.abilities.secondary.TryUse();
                state = NodeState.RUNNING;   

            }

            return state;
        }
    }
}
