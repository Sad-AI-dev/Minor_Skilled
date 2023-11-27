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
        bool gameEnded = false;

        public MeleeGruntWalkToTarget(Transform transform, Agent agent, NavMeshAgent navAgent)
        {
            this.transform = transform;
            this.agent = agent;
            this.navAgent = navAgent;

            EventBus<GameEndEvent>.AddListener(OnGameEnd);
            
        }

        public override NodeState Evaluate()
        {
            if (GetData("GameEnded") == null) SetData("GameEnded", false);
            if (GetData("Target") == null) { SetTarget(); }
            target = (Transform)GetData("Target");

            if (target == null)
            {
                state = NodeState.FAILURE;
            }
            //else go to target
            else
            {
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
            if (!(bool)GetData("GameEnded"))
            {
                SetData("Target", GameStateManager.instance.player.transform);
            }
        }
        void OnGameEnd(GameEndEvent gameEndEvent)
        {
            SetData("GameEnded", true);
            EventBus<GameEndEvent>.RemoveListener(OnGameEnd);
        }
    }
}
