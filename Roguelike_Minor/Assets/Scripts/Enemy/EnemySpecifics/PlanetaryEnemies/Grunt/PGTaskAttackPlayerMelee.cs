using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Enemy.Core;
using Game.Core;

namespace Game.Enemy
{
    public class PGTaskAttackPlayerMelee : BT_Node
    {
        private Transform transform;
        private Agent enemyAgent;
        private float distanceToTarget;

        public PGTaskAttackPlayerMelee(Transform transform, Agent enemyAgent)
        {
            this.transform = transform;
            this.enemyAgent = enemyAgent;
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
