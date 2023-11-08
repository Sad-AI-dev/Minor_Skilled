using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Enemy.Core;
using System.Threading.Tasks;

namespace Game.Enemy {
    public class MeleeGruntChooseAttack : BT_Node
    {
        bool chosen = false;
        int RandomOneZero;
        Transform transform;

        public MeleeGruntChooseAttack(Transform transform)
        {
            this.transform = transform;
        }

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
                    parent.SetData("Ranged", false);
                    chosen = true;
                }
                parent.SetData("Target", GameStateManager.instance.player.transform);
                if (GetData("DistanceToTarget") == null) CheckDistance();
            }

            if (chosen)
            {
                state = NodeState.FAILURE;
            }

            return state;
        }

        private async void CheckDistance()
        {
            Transform target = (Transform)GetData("Target");

            while (transform != null)
            {
                if (target != null && transform != null)
                {
                    parent.SetData("DistanceToTarget", Vector3.Distance(transform.position, target.position));
                }
                await Task.Delay(2);
            }
        }
    }
}
