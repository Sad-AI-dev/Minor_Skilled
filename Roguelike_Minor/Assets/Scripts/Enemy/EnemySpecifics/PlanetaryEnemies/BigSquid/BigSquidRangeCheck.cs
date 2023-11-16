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
            if (GetData("RandomFireRange") == null) parent.parent.SetData("RandomFireRange", Random.Range(BigSquidTree.FireRangeMin, BigSquidTree.FireRangeMax));

            if((float)GetData("DistanceToTarget") > (int)GetData("RandomFireRange"))
            {
                state = NodeState.FAILURE;
                return state;
            }
            else
            {
                Vector3 dir = (target.position - transform.position).normalized;

                RaycastHit hit;
                if (Physics.Raycast(transform.position, dir, out hit, Mathf.Infinity))
                {
                    if (hit.transform.CompareTag("Player"))
                    {
                        state = NodeState.SUCCESS;
                    }
                    else state = NodeState.FAILURE;
                }
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

