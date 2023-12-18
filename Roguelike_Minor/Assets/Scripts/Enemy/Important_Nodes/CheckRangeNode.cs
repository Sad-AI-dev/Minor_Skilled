using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Game.Enemy.Core;
using Game.Core;
using System.Threading.Tasks;

namespace Game.Enemy {
    public class CheckRangeNode : BT_Node
    {
        float distanceToCheck;

        public CheckRangeNode(Transform transform, float distanceToCheck)
        {
            this.transform = transform;
            this.distanceToCheck = distanceToCheck;
        }

        public override NodeState Evaluate()
        {
            state = NodeState.FAILURE;

            //Get / set target
            if (GetData("Target") == null) SetTarget(GameStateManager.instance.player.transform);
            target = (Transform)GetData("Target");

            //Set distance to target
            if (GetData("DistanceToTarget") == null) DistanceToTarget();

            if((float)GetData("DistanceToTarget") < distanceToCheck)
            {
                state = NodeState.SUCCESS;
            }

            return state;
        }

        async void DistanceToTarget()
        {
            while(target != null && transform != null)
            {
                SetDistanceToTarget(Vector3.Distance(transform.position, target.position));

                await Task.Delay(1);
            }
        }
    }
}
