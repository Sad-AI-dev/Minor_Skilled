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
                if (GetData("SecificRange") == null) SetEnemySpecificRange();
                if ((float)GetData("DistanceToTarget") <= (float)GetData("SecificRange"))
                {
                    state = NodeState.SUCCESS;
                    return state;
                }
                else state = NodeState.FAILURE;
            }

            return state;
        }

        void SetEnemySpecificRange()
        {
            parent.parent.SetData("SecificRange", Random.Range(MeleeGruntTree.rangedAttackRange, MeleeGruntTree.rangedAttackRange/2));
            Debug.Log((float)GetData("SecificRange"));
        }
    }
}
