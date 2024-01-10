using Game.Enemy.Core;
using Game.Core;
using UnityEngine;

namespace Game.Enemy {
    public class UseSecondaryNode : BT_Node
    {
        public UseSecondaryNode(Agent agent)
        {
            this.agent = agent;
        }

        public override NodeState Evaluate()
        {
            state = NodeState.SUCCESS;

            Debug.Log("Using Secondary");
            agent.abilities.secondary.TryUse();

            return state;
        }
    }
}
