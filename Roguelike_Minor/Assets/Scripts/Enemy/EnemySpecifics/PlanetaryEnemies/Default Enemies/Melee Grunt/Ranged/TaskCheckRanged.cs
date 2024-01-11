using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Enemy.Core;
using System.Threading.Tasks;

namespace Game.Enemy
{
    public class TaskCheckRanged : CheckRangeNode
    {
        bool ranged;

        public TaskCheckRanged(Agent agent, float distanceToCheck, bool ranged) : base(agent, distanceToCheck)
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

            state = base.Evaluate();
            
            return state;
        }

    }
}
