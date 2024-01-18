using UnityEngine;
using Game.Enemy.Core;
using UnityEngine.AI;
using Game.Core;

namespace Game.Enemy {
    public class MoveToTargetNavMeshNode : BT_Node
    {
        public MoveToTargetNavMeshNode(NavMeshAgent navAgent, Agent agent)
        {
            this.navAgent = navAgent;
            this.agent = agent;
        }

        public override NodeState Evaluate()
        {
            if (GetData("Target") == null) { SetTarget(GameStateManager.instance.player.transform); }
            target = (Transform)GetData("Target");

            if (target == null)
            {
                state = NodeState.FAILURE;
            }
            //else go to target
            else
            {
                state = NodeState.RUNNING;
                navAgent.speed = agent.stats.walkSpeed;
                navAgent.isStopped = false;
                navAgent.SetDestination(target.position);
            }

            return state;
        }
    }
}
