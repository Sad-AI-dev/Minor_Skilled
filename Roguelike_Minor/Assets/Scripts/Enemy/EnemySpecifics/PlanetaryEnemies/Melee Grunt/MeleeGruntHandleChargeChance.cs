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
        Transform transform, target;


        public MeleeGruntHandleChargeChance(Agent agent, NavMeshAgent navAgent, Transform transform)
        {
            this.transform = transform;
            this.agent = agent;
            this.navAgent = navAgent;
        }

        public override NodeState Evaluate()
        {
            if ((Transform)GetData("Target"))
            {
                agent.abilities.special.TryUse();
                state = NodeState.RUNNING;
            }
            else state = NodeState.FAILURE;

            return state;
        }
    }
}
