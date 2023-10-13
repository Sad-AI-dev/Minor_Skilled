using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Enemy.Core;
using UnityEngine.AI;
using Game.Core;

namespace Game.Enemy {
    public class PGTree : Core.Tree
    {
        //STATIC VARIABLES
        public static float speed = 4;
        public static float FOVRange = 13;
        public static float meleeAttackRange = 1.6f;
        public static float rangedAttackRange = 7f;
        public static int EnemiesInRangeOfPlayer = 0;

        //PUBLIC VARIABLE
        public NavMeshAgent agent;
        public LayerMask enemyLayerMask;
        public Agent enemyAgent;
        protected override void Start()
        {
            base.Start();
            agent.enabled = true;
        }

        protected override BT_Node SetupTree()
        {
            BT_Node root = new Sequence(
                new List<BT_Node>
                {
                    new PGTaskGoToTarget(transform, agent, enemyLayerMask, enemyAgent), // If enemy in range, go to enemy
                    new Selector( new List<BT_Node>{
                        new PGTaskAttackPlayerRanged(transform, enemyAgent, enemyLayerMask), // Attack ranged if player already has multiple close by
                        new PGTaskAttackPlayerMelee(transform, enemyAgent) // Attack the player untill its dead
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
