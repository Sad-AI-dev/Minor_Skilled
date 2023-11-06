using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Enemy.Core;
using System.Threading.Tasks;

namespace Game.Enemy
{
    public class TaskCheckMelee: BT_Node
    {
        bool ranged;
        float melee, semi, range;
        public TaskCheckMelee(bool ranged)
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
                if ((float)GetData("DistanceToTarget") <= melee)
                {
                    state = NodeState.SUCCESS;
                }
                else state = NodeState.FAILURE;
            }

            return state;
        }
    }
}
