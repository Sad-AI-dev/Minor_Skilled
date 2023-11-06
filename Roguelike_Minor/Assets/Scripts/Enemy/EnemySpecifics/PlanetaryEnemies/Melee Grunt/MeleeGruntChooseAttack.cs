using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Enemy.Core;

namespace Game.Enemy {
    public class MeleeGruntChooseAttack : BT_Node
    {
        bool chosen = false;
        int RandomOneZero;
        public override NodeState Evaluate()
        {
            if (!chosen)
            {
                RandomOneZero = Random.Range(0, 2);
                RandomOneZero = 1;
                if (RandomOneZero == 0)
                {
                    parent.SetData("Ranged", true);
                    chosen = true;
                }
                if (RandomOneZero == 1)
                {
                    parent.SetData("Ranged", true);
                    chosen = false;
                }
                parent.SetData("Target", GameStateManager.instance.player);
            }

            if (chosen)
            {
                state = NodeState.FAILURE;
                Debug.Log("Chose attack " + RandomOneZero);
            }

            return state;
        }
    }
}
