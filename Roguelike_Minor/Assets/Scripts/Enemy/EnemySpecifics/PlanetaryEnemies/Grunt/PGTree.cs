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
            agent.speed = GetComponent<Agent>().stats.walkSpeed;
            agent.enabled = true;
        }

        protected override BT_Node SetupTree()
        {
            BT_Node root = new Selector(
                new List<BT_Node>
                {
                    new Sequence( new List<BT_Node>
                    {
                        new CheckEnemyRangedAttack(transform, enemyLayerMask), //check if in range for ranged attack + melee check
                        new PGTaskAttackPlayerRanged(transform, enemyAgent, agent), // Attack ranged;
                    }), //Check for ranged attack
                    new Sequence( new List<BT_Node>
                    {
                        new CheckEnemyMeleeAttack(enemyAgent), // Check if in melee range
                        new PGTaskAttackPlayerMelee(transform, enemyAgent, agent) // Attack Melee
                    }), //Check for melee attack
                    new PGTaskGoToTarget(agent) // Go to target
                }
            ); ;
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
