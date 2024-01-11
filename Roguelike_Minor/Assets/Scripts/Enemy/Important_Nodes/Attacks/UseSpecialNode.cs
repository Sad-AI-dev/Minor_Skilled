using Game.Enemy.Core;
using Game.Core;

namespace Game.Enemy {
    public class UseSpecialNode : BT_Node
    {
        public UseSpecialNode(Agent agent)
        {
            this.agent = agent;
        }

        public override NodeState Evaluate()
        {
            state = NodeState.SUCCESS;

            agent.abilities.special.TryUse();

            return state;
        }
    }
}
