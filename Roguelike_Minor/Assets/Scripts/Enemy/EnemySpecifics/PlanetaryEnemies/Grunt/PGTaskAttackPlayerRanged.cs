using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Enemy.Core;
using Game.Core;

namespace Game.Enemy {
    public class PGTaskAttackPlayerRanged : BT_Node
    {
        Transform transform;
        Agent enemyAgent;
        LayerMask enemyLayerMask;
        private float distanceToTarget;

        public PGTaskAttackPlayerRanged(Transform transform, Agent enemyAgent, LayerMask enemyLayerMask)
        {
            this.transform = transform;
            this.enemyAgent = enemyAgent;
            this.enemyLayerMask = enemyLayerMask;
        }

        public override NodeState Evaluate()
        {
            Transform target = (Transform)GetData("Target");
            if (GetData("DistanceToTarget") != null)
            {
                distanceToTarget = (float)GetData("DistanceToTarget");
            }

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
                Debug.Log("shooting");
                enemyAgent.abilities.secondary.TryUse();
                state = NodeState.RUNNING;   
            }

            return state;
        }
    }
}
