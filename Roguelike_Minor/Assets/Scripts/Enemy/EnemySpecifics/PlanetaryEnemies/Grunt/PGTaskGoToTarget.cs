using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Enemy.Core;
using UnityEngine.AI;

namespace Game.Enemy
{
    public class PGTaskGoToTarget : BT_Node
    {
        Transform transform;
        NavMeshAgent agent;
        LayerMask playerLayerMask;

        public PGTaskGoToTarget(Transform transform, NavMeshAgent agent, LayerMask playerLayerMask)
        {
            this.transform = transform;
            this.agent = agent;
            this.playerLayerMask = playerLayerMask;
        }

        public override NodeState Evaluate()
        {
            Transform target = (Transform)GetData("Target");
            Collider[] col = Physics.OverlapSphere(
                    transform.position, PGTree.attackRange, playerLayerMask);

            if (col.Length <= 0 && target != null)
            {
                agent.SetDestination(target.position);
                state = NodeState.RUNNING;
            }
            else if(target == null)
            {
                state = NodeState.FAILURE;
            }
            else
            {
                state = NodeState.SUCCESS;
            }

            return state;
        }
    }
}
