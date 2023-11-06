using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Enemy.Core;
using System.Threading.Tasks;

namespace Game.Enemy
{
    public class TaskCheckSemi : BT_Node
    {
        bool ranged;
        int chargeChancePercent;
        public TaskCheckSemi(bool ranged, int chargeChancePercent)
        {
            this.ranged = ranged;
        }

        public override NodeState Evaluate()
        {
            if ((bool)GetData("Ranged") != ranged)
            {
                state = NodeState.FAILURE;
                return state;
            }
            else
            {
                if ((float)GetData("DistanceToTarget") <= MeleeGruntTree.semiMeleeAttackRange && 
                    (float)GetData("DistanceToTarget") > MeleeGruntTree.meleeAttackRange)
                {
                    //Get random number.
                    int random = Random.Range(1, 101);

                    //Fail charge if true and keep walking;
                    if (random <= chargeChancePercent)
                    {
                        state = NodeState.SUCCESS;
                        return state;
                    }
                }
                else state = NodeState.FAILURE;
            }

            return state;
        }
    }
}
