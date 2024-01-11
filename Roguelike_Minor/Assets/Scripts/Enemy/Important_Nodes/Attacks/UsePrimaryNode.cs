using Game.Enemy.Core;
using Game.Core;

namespace Game.Enemy {
    public class UsePrimaryNode : BT_Node
    {
        public UsePrimaryNode(Agent agent)
        {
            this.agent = agent;
        }

        public override NodeState Evaluate()
        {
            state = NodeState.SUCCESS;

            agent.abilities.primary.TryUse();

            return state;
        }
    }
}
