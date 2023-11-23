using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Enemy.Core;
using Game.Core;
using System.Threading.Tasks;

namespace Game.Enemy {
    public class SmallSquidCheckRange : BT_Node
    {
        Transform transform;
        Transform target;
        Agent agent;

        public SmallSquidCheckRange(Transform transform, Agent agent)
        {
            this.transform = transform;
            this.agent = agent;

            EventBus<GameEndEvent>.AddListener(ClearTarget);
        }

        public override NodeState Evaluate()
        {
            //Get Target and distance to target
            if(GetData("Target") == null)
            {
                SetTarget();
                target = (Transform)GetData("Target");
            }
            if(GetData("DistanceToTarget") == null)
            {
                CheckDistance();
            }

            if (target != null && transform != null)
            {
                float distanceToTarget = (float)GetData("DistanceToTarget");
                Vector3 direction = ((target.position + Vector3.up ) - transform.position).normalized;

                if (distanceToTarget > SmallSquidTree.AttackRange)
                {
                    if (GetData("Chasing") != null && (bool)GetData("Chasing")) SetData("Chasing", false);
                    state = NodeState.FAILURE;
                    return state;
                }
                else
                {
                    RaycastHit hit;
                    if (Physics.SphereCast(transform.position, 0.5f, direction, out hit, distanceToTarget))
                    {
                        if (!hit.transform.CompareTag("Player"))
                        {
                            state = NodeState.FAILURE;
                            return state;
                        }
                        if (GetData("Chasing") == null) SetData("Chasing", true);
                        else state = NodeState.SUCCESS;
                    }
                }
            }

            return state;
        }

        void SetTarget()
        {
            if (GameStateManager.instance.player != null)
            {
                SetData("Target", GameStateManager.instance.player.transform);
                target = (Transform)GetData("Target");
            }
        }
        private async void CheckDistance()
        {
            if (GetData("Target") != null) target = (Transform)GetData("Target");

            while (transform != null && target != null)
            {
                if (target != null && transform != null)
                {
                    SetData("DistanceToTarget", Vector3.Distance(transform.position, target.position));
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
