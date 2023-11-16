using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Game.Enemy.Core;
using Game.Core;
using System.Threading.Tasks;

namespace Game.Enemy {
    public class BigSquidRangeCheck : BT_Node
    {
        private Transform transform;
        private Transform target;

        public BigSquidRangeCheck(Transform transform)
        {
            this.transform = transform;
            EventBus<GameEndEvent>.AddListener(ClearTarget);
        }

        public override NodeState Evaluate()
        {
            if (GetData("Target") == null) SetTarget();
            if (GetData("DistanceToTarget") == null) CheckDistance();

            if((float)GetData("DistanceToTarget") > BigSquidTree.FireRange)
            {
                state = NodeState.FAILURE;
                return state;
            }
            else
            {
                state = NodeState.SUCCESS;
            }

            return state;
        }

        void SetTarget()
        {
            parent.parent.SetData("Target", GameStateManager.instance.player.transform);
            target = (Transform)GetData("Target");
        }
        private async void CheckDistance()
        {
            while (transform != null && target != null)
            {
                if (target != null && transform != null)
                {
                    parent.parent.SetData("DistanceToTarget", Vector3.Distance(transform.position, target.position));
                }
                await Task.Delay(2);
            }
        }
        private void ClearTarget(GameEndEvent eventData)
        {
            ClearData("Target");
            target = null;
            EventBus<GameEndEvent>.RemoveListener(ClearTarget);
        }
    }
}

