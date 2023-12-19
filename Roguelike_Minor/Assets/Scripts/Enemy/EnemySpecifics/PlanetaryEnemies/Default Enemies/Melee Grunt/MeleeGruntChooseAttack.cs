using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Enemy.Core;
using System.Threading.Tasks;
using Game.Core;

namespace Game.Enemy {
    public class MeleeGruntChooseAttack : BT_Node
    {
        bool chosen = false;
        int RandomOneZero;

        public MeleeGruntChooseAttack(Transform transform)
        {
            this.transform = transform;
            EventBus<GameEndEvent>.AddListener(HandleClearTarget);
        }

        public override NodeState Evaluate()
        {
            if (!chosen)
            {
                RandomOneZero = Random.Range(0, 2);

                if (RandomOneZero == 0)
                {
                    SetData("Ranged", true);
                    chosen = true;
                }
                if (RandomOneZero == 1)
                {
                    SetData("Ranged", false);
                    chosen = true;
                }
                SetTarget(GameStateManager.instance.player.transform);
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
            if(GetData("Target") != null) target = (Transform)GetData("Target");

            while (transform != null && target != null)
            {
                if (target != null && transform != null)
                {
                    SetData("DistanceToTarget", Vector3.Distance(transform.position, target.position));
                }
                await Task.Delay(2);
            }
        }

        private void HandleClearTarget(GameEndEvent eventData)
        {
            ClearData("Target");
            target = null;
            EventBus<GameEndEvent>.RemoveListener(HandleClearTarget);
        }
    }
}
