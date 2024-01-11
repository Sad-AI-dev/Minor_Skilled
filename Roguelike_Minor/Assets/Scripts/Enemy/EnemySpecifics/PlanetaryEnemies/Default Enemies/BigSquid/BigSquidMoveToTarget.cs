using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Game.Enemy.Core;
using Game.Core;

namespace Game.Enemy {
    public class BigSquidMoveToTarget : MoveToTargetFlyingNode
    {
        public BigSquidMoveToTarget(Transform transform, Agent agent, Rigidbody rb, float rotationSpeedMoving, float randomMinimumHeight) : base(transform, agent, rb, rotationSpeedMoving, randomMinimumHeight)
        {

        }

        public override NodeState Evaluate()
        {
            if(GetData("Targeting") != null && (bool)GetData("Targeting"))
            {
                state = NodeState.RUNNING;
                return state;
            }

            state = base.Evaluate();

            return state;
        }
    }
}
