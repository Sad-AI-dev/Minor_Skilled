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
        public static float FOVRange = 10;
        public static float meleeAttackRange = 1.2f;
        public static float rangedAttackRange = 6f;
        public static int EnemiesInRangeOfPlayer = 0;

        //VARIABLE
        public NavMeshAgent agent;
        public LayerMask layerMask;
        public LayerMask enemyLayerMask;
        public Agent enemyAgent;

        protected override BT_Node SetupTree()
        {
            BT_Node root = new Sequence(
                new List<BT_Node>
                {
                    new PGTaskGoToTarget(transform, agent, layerMask, enemyLayerMask), // If enemy in range, go to enemy
                    new Selector( new List<BT_Node>{
                        new PGTaskAttackPlayerRanged(transform, enemyAgent, layerMask, enemyLayerMask), // Attack ranged if player already has multiple close by
                        new PGTaskAttackPlayerMelee(transform, enemyAgent, layerMask) // Attack the player untill its dead
                    })
                }
            );
            return root;
        }

        private void OnDrawGizmos()
        {
            // Show the range of attack
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, meleeAttackRange);

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, rangedAttackRange);
        }
    }
}
