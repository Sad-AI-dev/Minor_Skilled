using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Enemy.Core;
using Game.Core;
using UnityEngine.AI;

namespace Game.Enemy {
    public class MeleeGruntWalkToTarget : MoveToTargetNavMeshNode
    {
        public MeleeGruntWalkToTarget(Transform transform, Agent agent, NavMeshAgent navAgent) : base(navAgent, agent)
        {
            this.transform = transform;
            this.navAgent = navAgent;
        }

        public override NodeState Evaluate()
        {
            if (GetData("GameEnded") == null) SetData("GameEnded", false);

            state = base.Evaluate();

            if(state == NodeState.RUNNING)
            {
                if (GetData("Charging") == null || !(bool)GetData("Charging")) navAgent.isStopped = false;
                if (GetData("Charging") != null && (bool)GetData("Charging")) transform.LookAt(target);
            }

            return state;
        }
    }
}
