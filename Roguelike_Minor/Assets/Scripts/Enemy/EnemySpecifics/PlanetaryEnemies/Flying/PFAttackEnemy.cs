using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Enemy.Core;

namespace Game.Enemy {
    public class PFAttackEnemy : BT_Node
    {
        private Transform transform;
        private Transform target;

        public PFAttackEnemy(Transform transform)
        {
            this.transform = transform;
        }

        public override NodeState Evaluate()
        {
            //If no target - Fail
            target = (Transform)GetData("Target");

            if (target == null)
            {
                state = NodeState.FAILURE;
            }
            else
            {
                //Handle Attacking
                state = NodeState.RUNNING;
            }
            return state;
        }
    }
}
