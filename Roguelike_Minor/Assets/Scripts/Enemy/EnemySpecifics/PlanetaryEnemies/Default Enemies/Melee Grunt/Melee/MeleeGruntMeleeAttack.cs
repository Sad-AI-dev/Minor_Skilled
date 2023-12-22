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
            agent.abilities.primary.TryUse();
            state = NodeState.RUNNING;             

            return state;
        }
    }
}
