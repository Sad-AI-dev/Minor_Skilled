using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Enemy.Core;
using Game.Core;
using UnityEngine.AI;

namespace Game.Enemy {
    public class MeleeGruntWalkToTarget : BT_Node
    {
        Transform transform;
        Transform target;
        Agent agent;
        NavMeshAgent navAgent;

        public MeleeGruntWalkToTarget(Transform transform, Agent agent, NavMeshAgent navAgent)
        {
            this.transform = transform;
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
                Debug.Log("Walking to target");
                if(GetData("Charging") != null && (bool)GetData("Charging") == false) navAgent.isStopped = false;
                if (GetData("Charging") != null && (bool)GetData("Charging") == true) transform.LookAt(target);
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
