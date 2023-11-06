using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Enemy.Core;
using Game.Core;
using UnityEngine.AI;

namespace Game.Enemy {
    public class MeleeGruntWalkToTarget : BT_Node
    {
        Transform target;
        Agent agent;
        NavMeshAgent navAgent;

        public MeleeGruntWalkToTarget(Agent agent, NavMeshAgent navAgent)
        {
            this.agent = agent;
            this.navAgent = navAgent;
        }

        public override NodeState Evaluate()
        {
            if (GetData("Target") == null) { SetTarget(); }
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
                navAgent.SetDestination(target.position);
            }


            return state;
        }

        void SetTarget()
        {
            parent.SetData("Target", GameStateManager.instance.player.transform);
        }
    }
}
