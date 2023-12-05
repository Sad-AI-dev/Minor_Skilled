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
        public BigSquidRangeCheck(Transform transform)
        {
            this.transform = transform;
            EventBus<GameEndEvent>.AddListener(DoClearTarget);
        }

        public override NodeState Evaluate()
        {
            if (GetData("Target") == null) SetTarget(GameStateManager.instance.player.transform);
            target = (Transform)GetData("Target");
            if (GetData("DistanceToTarget") == null) CheckDistance();
            if (GetData("RandomFireRange") == null) SetData("RandomFireRange", Random.Range(BigSquidTree.FireRangeMin, BigSquidTree.FireRangeMax));

            if(GetData("DistanceToTarget") == null)
            {
                state = NodeState.FAILURE;
                return state;
            }
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

        private async void CheckDistance()
        {
            while (transform != null && target != null)
            {
                if (target != null && transform != null)
                {
                    SetData("DistanceToTarget", Vector3.Distance(transform.position, target.position));
                }
                await Task.Delay(2);
            }
        }
        private void DoClearTarget(GameEndEvent eventData)
        {
            ClearData("Target");
            target = null;
            EventBus<GameEndEvent>.RemoveListener(DoClearTarget);
        }
    }
}

