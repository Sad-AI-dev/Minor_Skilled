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
        public MeleeGruntMeleeAttack(Agent agent, NavMeshAgent navAgent, Transform transform) 
        {
            this.agent = agent;
            this.navAgent = navAgent;
            this.transform = transform;
        }

        //melee attack
        Vector3 targetPostition;
        public override NodeState Evaluate()
        {
            Debug.Log("Handling melee");
            if ((Transform)GetData("Target"))
            {
                navAgent.isStopped = true;
                target = (Transform)GetData("Target");
                targetPostition = new Vector3(target.position.x, transform.position.y, target.position.z);
                transform.LookAt(targetPostition);
                agent.abilities.primary.TryUse();
                state = NodeState.RUNNING;             
            }
            else state = NodeState.FAILURE;

            return state;
        }
    }
}
