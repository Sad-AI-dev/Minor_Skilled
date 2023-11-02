using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Enemy.Core;

namespace Game.Enemy
{
    public class MeleeGruntChooseAttack : BT_Node
    {
        bool chosen;

        public override NodeState Evaluate()
        {
            int RandomOneZero = Random.Range(0, 2);

            if(RandomOneZero == 0)
            {
                parent.SetData("Ranged", true);
                chosen = true;
            }
            if(RandomOneZero == 1)
            {
                parent.SetData("Ranged", false);
                chosen = true;
            }

            if (chosen)
            {
                state = NodeState.SUCCESS;
            }

            return state;
        }
    }
}
