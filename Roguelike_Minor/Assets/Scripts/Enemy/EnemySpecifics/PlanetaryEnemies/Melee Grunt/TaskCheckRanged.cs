using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Enemy.Core;
using System.Threading.Tasks;

namespace Game.Enemy
{
    public class TaskCheckRanged : BT_Node
    {
        bool ranged;

        public TaskCheckRanged(bool ranged)
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
                if ((float)GetData("DistanceToTarget") <= MeleeGruntTree.rangedAttackRange 
                    && (float)GetData("DistanceToTarget") > MeleeGruntTree.semiMeleeAttackRange)
                {
                    state = NodeState.SUCCESS;
                    return state;
                }
                else state = NodeState.FAILURE;
            }

            return state;
        }
    }
}
