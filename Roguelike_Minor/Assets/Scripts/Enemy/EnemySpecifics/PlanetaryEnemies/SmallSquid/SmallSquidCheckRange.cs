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

            float distanceToTarget = (float)GetData("DistanceToTarget");
            Vector3 direction = (target.position - transform.position).normalized;

            if(distanceToTarget > SmallSquidTree.AttackRange)
            {
                state = NodeState.FAILURE;
                return state;
            }
            else
            {
                RaycastHit hit;
                if (Physics.SphereCast(transform.position, 0.5f, direction, out hit, distanceToTarget))
                {
                    Debug.Log(hit.transform.name);
                    if (!hit.transform.CompareTag("Player"))
                    {
                        state = NodeState.FAILURE;
                        return state;
                    }
                    else state = NodeState.SUCCESS;
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
            Transform target = (Transform)GetData("Target");
            while (transform != null)
            {
                if (target != null && transform != null)
                {
                    parent.parent.SetData("DistanceToTarget", Vector3.Distance(transform.position, target.position));
                }
                await Task.Delay(2);
            }
        }
    }
}
