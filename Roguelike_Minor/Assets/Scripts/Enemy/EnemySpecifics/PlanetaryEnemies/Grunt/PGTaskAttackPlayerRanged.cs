using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Enemy.Core;
using Game.Core;

namespace Game.Enemy
{
    public class PGTaskAttackPlayerRanged : BT_Node
    {
        Transform transform;
        Agent enemyAgent;
        LayerMask playerLayerMask, enemyLayerMask;

        public PGTaskAttackPlayerRanged(Transform transform, Agent enemyAgent, LayerMask playerLayerMask, LayerMask enemyLayerMask)
        {
            this.transform = transform;
            this.enemyAgent = enemyAgent;
            this.playerLayerMask = playerLayerMask;
            this.enemyLayerMask = enemyLayerMask;
        }

        public override NodeState Evaluate()
        {
            Transform target = (Transform)GetData("Target");
            Collider[] colMelee = Physics.OverlapSphere(
                    transform.position, PGTree.meleeAttackRange, playerLayerMask);

            //If target null: Fail
            //If in melee ranged: Fail
            //else Running

            if (target == null)
            {
                //if (enemyAgent.transform.gameObject.name == "MainTestGrunt") Debug.Log("Target is not found: " + (Transform)GetData("Target"));
                state = NodeState.FAILURE;
            }
            else if (PGTree.EnemiesInRangeOfPlayer < 3 || colMelee.Length > 0)
            {
                state = NodeState.FAILURE;
            }
            else
            {
                Debug.Log("attacking Ranged");
                enemyAgent.abilities.secondary.TryUse();
                state = NodeState.RUNNING;
            }

            return state;
        }
    }
}