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

        public CheckRangeNode(Agent agent, float distanceToCheck)
        {
            this.agent = agent;
            this.distanceToCheck = distanceToCheck;
        }

        public override NodeState Evaluate()
        {
            state = NodeState.FAILURE;

            //Get / set target
            if (GetData("Target") == null) SetTarget(GameStateManager.instance.player.transform);
            target = (Transform)GetData("Target");

            //Set distance to target
            if (GetData("DistanceToTarget") == null) agent.StartCoroutine(DistanceToTargetCO(agent, agent.transform, target));

            if ((float)GetData("DistanceToTarget") < distanceToCheck)
            {
                state = NodeState.SUCCESS;
            }

            return state;
        }
    }
}
