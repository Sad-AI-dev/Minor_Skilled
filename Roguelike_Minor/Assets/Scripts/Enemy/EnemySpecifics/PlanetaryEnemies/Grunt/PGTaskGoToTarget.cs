using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Enemy.Core;
using UnityEngine.AI;
using Game.Systems;

namespace Game.Enemy
{
    public class PGTaskGoToTarget : BT_Node
    {
        Transform transform;
        NavMeshAgent agent;
        LayerMask playerLayerMask, enemyLayerMask;
        bool attackingMelee = false;
        Transform target;

        public PGTaskGoToTarget(Transform transform, NavMeshAgent agent, LayerMask playerLayerMask, LayerMask enemyLayerMask)
        {
            this.transform = transform;
            this.agent = agent;
            this.playerLayerMask = playerLayerMask;
            this.enemyLayerMask = enemyLayerMask;
        }

        public override NodeState Evaluate()
        {
            //Check if target already was found. otherwise add it
            if ((Transform)GetData("Target") == null) parent.SetData("Target", GameStateManager.instance.player.transform);
            target = (Transform)GetData("Target");


            Collider[] colMelee = Physics.OverlapSphere(
                    transform.position, PGTree.meleeAttackRange, playerLayerMask);

            Collider[] colRanged = Physics.OverlapSphere(
                    transform.position, PGTree.rangedAttackRange, playerLayerMask);

            //If no target, fail
            if(target == null)
            {
                state = NodeState.FAILURE;
            }
            //else if mellee in ranged, success
            else if (colMelee.Length > 0)
            {
                agent.SetDestination(transform.position);
                transform.LookAt(target);
                if (!attackingMelee)
                {
                    attackingMelee = true;
                    PGTree.EnemiesInRangeOfPlayer++;
                }
                state = NodeState.SUCCESS;
            }
            //else if ranged in range && meleecounter > 3, success
            else if(colRanged.Length > 0 && PGTree.EnemiesInRangeOfPlayer >= 3)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, target.position - transform.position, out hit, Mathf.Infinity))
                {

                    if (hit.transform.tag == "Player")
                    {
                        agent.SetDestination(transform.position);
                        transform.LookAt(target);
                        state = NodeState.SUCCESS;
                    }
                    else
                    {
                        state = NodeState.RUNNING;
                    }
                }
            }
            //else go to target
            else
            {
                agent.SetDestination(target.position);
                state = NodeState.RUNNING;
            }

            return state;
        }
    }
}
