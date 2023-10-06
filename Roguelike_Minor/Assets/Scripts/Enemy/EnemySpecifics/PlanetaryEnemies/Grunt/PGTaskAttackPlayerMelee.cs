using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Enemy.Core;
using Game.Core;

namespace Game.Enemy
{
    public class PGTaskAttackPlayerMelee : BT_Node
    {
        Transform transform;
        Agent enemyAgent;
        LayerMask playerLayerMask;

        public PGTaskAttackPlayerMelee(Transform transform, Agent enemyAgent, LayerMask playerLayerMask)
        {
            this.transform = transform;
            this.enemyAgent = enemyAgent;
            this.playerLayerMask = playerLayerMask;
        }

        public override NodeState Evaluate()
        {
            Collider[] col = Physics.OverlapSphere(
                    transform.position, PGTree.meleeAttackRange, playerLayerMask);

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
