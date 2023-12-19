using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Game.Enemy.Core;
using Game.Core;

namespace Game.Enemy
{
    public class Rock_BigSquid_MoveToTarget : BT_Node
    {
        public Rock_BigSquid_MoveToTarget(NavMeshAgent navAgent)
        {
            this.navAgent = navAgent;
        }

        public override NodeState Evaluate()
        {
            state = NodeState.RUNNING;

            if(target != (Transform)GetData("Target")) target = (Transform)GetData("Target");

            navAgent.SetDestination(target.position);

            return state;
        }
    }
}
