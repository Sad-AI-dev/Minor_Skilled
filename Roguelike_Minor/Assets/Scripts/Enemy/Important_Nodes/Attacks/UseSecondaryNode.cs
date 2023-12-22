using Game.Enemy.Core;
using Game.Core;

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

            agent.abilities.secondary.TryUse();

            return state;
        }
    }
}
