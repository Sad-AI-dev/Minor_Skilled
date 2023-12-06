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
        NavMeshAgent navAgent;

        public MeleeGruntMeleeAttack(Agent agent, NavMeshAgent navAgent, Transform transform) 
        {
            this.agent = agent;
            this.navAgent = navAgent;
            this.transform = transform;
        }

        //melee attack
        public override NodeState Evaluate()
        {
            if ((Transform)GetData("Target"))
            {
                navAgent.isStopped = true;
                Transform target = (Transform)GetData("Target");
                Vector3 targetPostition = new Vector3(target.position.x, transform.position.y, target.position.z);
                transform.LookAt(targetPostition);
                agent.abilities.primary.TryUse();
                state = NodeState.RUNNING;             
            }
            else state = NodeState.FAILURE;

            return state;
        }
    }
}
