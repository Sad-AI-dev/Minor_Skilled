using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Enemy.Core;
using Game.Core;
using UnityEngine.AI;

namespace Game.Enemy
{
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
            state = NodeState.RUNNING;

            //navAgent.SetDestination()

            return state;
        }
    }
}
