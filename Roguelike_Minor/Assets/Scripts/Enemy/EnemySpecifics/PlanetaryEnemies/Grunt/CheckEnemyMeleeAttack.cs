using UnityEngine;
using Game.Enemy.Core;
using Game.Core;

namespace Game.Enemy {
    public class CheckEnemyMeleeAttack : BT_Node
    {
        private float distanceToTarget;
        private bool attackingMelee = false;

        public CheckEnemyMeleeAttack(Agent enemyAgent)
        {
            enemyAgent.health.onDeath.AddListener(CheckRangeOnDeath);
        }

        public override NodeState Evaluate()
        {
            if (GetData("DistanceToTarget") != null)
            {
                distanceToTarget = (float)GetData("DistanceToTarget");
            }
            //If not in range, fail
            if(distanceToTarget > PGTree.meleeAttackRange)
            {
                state = NodeState.FAILURE;
                if (attackingMelee == true)
                {
                    attackingMelee = false;
                    PGTree.EnemiesInRangeOfPlayer--;
                }
            }
            //Else If target in range: Succeed
            else
            {
                state = NodeState.SUCCESS;
                if(attackingMelee == false)
                {
                    attackingMelee = true;
                    PGTree.EnemiesInRangeOfPlayer++;
                }
            }
            return state;
        }

        void CheckRangeOnDeath(HitEvent hit)
        {
            if (attackingMelee)
            {
                attackingMelee = false;
                PGTree.EnemiesInRangeOfPlayer--;
            }
        }
    }
}
