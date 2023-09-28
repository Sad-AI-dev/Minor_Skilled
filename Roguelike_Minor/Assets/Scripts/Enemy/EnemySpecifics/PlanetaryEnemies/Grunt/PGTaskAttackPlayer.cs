using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Enemy.Core;
using Game.Core;

namespace Game.Enemy
{
    public class PGTaskAttackPlayer : BT_Node
    {
        Transform transform;
        Agent enemyAgent;
        LayerMask playerLayerMask;

        public PGTaskAttackPlayer(Transform transform, Agent enemyAgent, LayerMask playerLayerMask)
        {
            this.transform = transform;
            this.enemyAgent = enemyAgent;
            this.playerLayerMask = playerLayerMask;
        }

        public override NodeState Evaluate()
        {
            Transform target = (Transform)GetData("Target");
            Collider[] col = Physics.OverlapSphere(
                    transform.position, PGTree.attackRange, playerLayerMask);

            if(col.Length > 0) 
            { 
                enemyAgent.abilities.primary.TryUse();
                state = NodeState.RUNNING;
            }
            else
            {
                state = NodeState.FAILURE;
            }

            return state;
        }
    }
}
