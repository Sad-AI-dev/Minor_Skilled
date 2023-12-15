using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Game.Enemy.Core;
using Game.Core;

namespace Game.Enemy {
    public class Rock_BigSquid_HandleMelee : BT_Node
    {
        public Rock_BigSquid_HandleMelee(Agent agent)
        {
            this.agent = agent;
        }

        public override NodeState Evaluate()
        {
            state = NodeState.RUNNING;

            agent.abilities.secondary.TryUse();

            return state;
        }
    }
}
