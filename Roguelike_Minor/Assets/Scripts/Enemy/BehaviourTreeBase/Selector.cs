using System.Collections;
using System.Collections.Generic;

namespace Game.Enemy.Core
{
    public class Selector : BT_Node
    {
        public Selector() : base() { }
        public Selector(List<BT_Node> children) : base(children) { }

        public override NodeState Evaluate()
        {
            foreach (var node in children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.RUNNING:
                        state = NodeState.RUNNING;
                        return state;
                    case NodeState.SUCCESS:
                        state = NodeState.SUCCESS;
                        return state;
                    case NodeState.FAILURE:
                        continue;
                    default:
                        continue;
                }
            }
            state = NodeState.FAILURE;
            return state;
        }
    }
}
