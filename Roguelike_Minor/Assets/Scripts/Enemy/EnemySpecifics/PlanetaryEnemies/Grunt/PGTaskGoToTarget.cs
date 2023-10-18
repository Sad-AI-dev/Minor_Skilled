using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Enemy.Core;
using UnityEngine.AI;

using Game.Core;

namespace Game.Enemy {
    public class PGTaskGoToTarget : BT_Node
    {
        private NavMeshAgent agent;
        private Transform target;

        public PGTaskGoToTarget(NavMeshAgent agent)
        {
            this.agent = agent;
        }

        public override NodeState Evaluate()
        {
            //Check if target already was found. otherwise add it
            if ((Transform)GetData("Target") == null) parent.SetData("Target", GameStateManager.instance.player.transform);
            target = (Transform)GetData("Target");     

            //If no target, fail
            if(target == null)
            {
                state = NodeState.FAILURE;
            }
            //else go to target
            else
            {
                agent.SetDestination(target.position);
                state = NodeState.RUNNING;
            }

            return state;
        }
       
        
    }
}
