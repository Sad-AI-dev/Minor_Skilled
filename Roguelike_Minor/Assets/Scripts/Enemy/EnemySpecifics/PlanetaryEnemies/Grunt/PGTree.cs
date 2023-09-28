using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Enemy.Core;
using UnityEngine.AI;
using Game.Core;

namespace Game.Enemy
{
    public class PGTree : Core.Tree
    {
        //STATIC
        public static float speed = 4;
        public static float FOVRange = 6;
        public static float attackRange = 1.3f;

        //VARIABLE
        public NavMeshAgent agent;
        public LayerMask layerMask;
        public Agent enemyAgent;

        protected override BT_Node SetupTree()
        {
            BT_Node root = 
                new Selector(
                    new List<BT_Node> {
                        new Sequence( new List<BT_Node>{
                            new CheckForTargetInRange(transform, layerMask),
                            new PGTaskGoToTarget(transform, agent, layerMask),
                            new PGTaskAttackPlayer(transform, enemyAgent, layerMask)
                        }),
                        new PGTaskPatrol(transform, agent)
                    }
            );
            return root;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, FOVRange);
        }
    }
}
