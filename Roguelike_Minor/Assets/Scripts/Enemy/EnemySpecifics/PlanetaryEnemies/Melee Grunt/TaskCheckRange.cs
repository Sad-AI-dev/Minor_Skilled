using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Enemy.Core;
using System.Threading.Tasks;

namespace Game.Enemy {
    public class TaskCheckRange : BT_Node
    {
        bool ranged;
        float melee, semi, range;
        public TaskCheckRange(bool ranged, float melee, float semi, float range)
        {
            this.ranged = ranged;
        }

        public override NodeState Evaluate()
        {
            if((bool)GetData("Ranged") != ranged)
            {
                state = NodeState.FAILURE;
                return state;
            }
            else
            {


                state = NodeState.SUCCESS;
            }

            return state;
        }
    }
}
