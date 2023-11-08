using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Enemy.Core;
using System.Threading.Tasks;
using UnityEngine.AI;
using Game.Core;

namespace Game.Enemy {
    public class MeleeGruntMeleeAttack : BT_Node
    {
        Agent agent;
        NavMeshAgent navAgent;
        public MeleeGruntMeleeAttack(Agent agent, NavMeshAgent navAgent) 
        {
            this.agent = agent;
            this.navAgent = navAgent;
        }

        //melee attack
        public override NodeState Evaluate()
        {
            if ((Transform)GetData("Target"))
            {
                navAgent.isStopped = true;
                agent.abilities.primary.TryUse();
                state = NodeState.RUNNING;             
            }
            else state = NodeState.FAILURE;

            return state;
        }
    }
}
