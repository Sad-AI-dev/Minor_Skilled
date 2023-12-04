using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Game.Enemy.Core;
using Game.Core;
using Game.Core.Data;


namespace Game.Enemy {
    public class Boss_MeleeGrunt_MoveToTarget : BT_Node
    {
        NavMeshAgent navAgent;

        public Boss_MeleeGrunt_MoveToTarget(Transform transform, Agent agent, NavMeshAgent navAgent)
        {
            this.transform = transform;
            this.agent = agent;
            this.navAgent = navAgent;
        }

        public override NodeState Evaluate()
        {
            state = NodeState.SUCCESS;

            if (GetData("Target") == null) SetTarget(GameStateManager.instance.player.transform);
            target = (Transform)GetData("Target");


            if(GetData("UsingAbility") != null && (bool)GetData("UsingAbility"))
            {
                navAgent.isStopped = true;
                return state;
            }

            navAgent.SetDestination(transform.position);

            return state;
        }
    }
}
