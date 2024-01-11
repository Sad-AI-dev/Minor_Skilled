using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using Game.Core;
using Game.Enemy.Core;

namespace Game.Enemy {
    public class BigSquidHandleAttack : BT_Node
    {
        public BigSquidHandleAttack(Transform transform, Agent agent)
        {
            this.agent = agent;
            this.transform = transform;
        }

        BigSquidPrimaryVars vars;
        public override NodeState Evaluate()
        {
            if(target != null)
            {
                //Handle Rotation
                agent.abilities.primary.TryUse();
            }

            return state;
        }

        
    }
}
