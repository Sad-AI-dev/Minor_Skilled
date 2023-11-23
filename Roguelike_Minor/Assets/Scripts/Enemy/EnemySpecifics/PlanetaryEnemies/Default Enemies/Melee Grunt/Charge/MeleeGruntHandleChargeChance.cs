using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Enemy.Core;
using System.Threading.Tasks;
using UnityEngine.AI;
using Game.Core;

namespace Game.Enemy {
    public class MeleeGruntHandleChargeChance : BT_Node
    {
        Agent agent;
        NavMeshAgent navAgent;
        Transform transform;

        public MeleeGruntHandleChargeChance(Agent agent, NavMeshAgent navAgent, Transform transform)
        {
            this.transform = transform;
            this.agent = agent;
            this.navAgent = navAgent;
        }

        public override NodeState Evaluate()
        {
            if (GetData("Target") != null)
            {
                Transform target = (Transform)GetData("Target");
                Vector3 targetPostition = new Vector3(target.position.x, transform.position.y, target.position.z);
                transform.LookAt(targetPostition);

                agent.abilities.special.TryUse();
                navAgent.isStopped = true;
                state = NodeState.RUNNING;
            }
            else state = NodeState.FAILURE;

            return state;
        }
    }
}
