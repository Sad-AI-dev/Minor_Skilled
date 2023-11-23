using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Game.Enemy.Core;
using Game.Core;
using Game.Core.Data;
using System.Threading.Tasks;

namespace Game.Enemy {
    public class Boss_BigSquid_CheckRange : BT_Node
    {
        Transform transform;
        Transform target;

        float distanceToTarget;
        Vector3 sphereCastDirection;

        public Boss_BigSquid_CheckRange(Transform transform)
        {
            this.transform = transform;
        }

        public override NodeState Evaluate()
        {
            state = NodeState.SUCCESS;
            if (GetData("Target") == null) SetTarget(GameStateManager.instance.player.transform);
            target = (Transform)GetData("Target");
            if (GetData("DistanceToTarget") == null) CheckDistance();
            distanceToTarget = (float)GetData("DistanceToTarget");

            //If not in range, fail
            if (distanceToTarget > Boss_BigSquidTree.AttackRange)
            {
                SetData("Patroling", false);
                state = NodeState.FAILURE;
                return state;
            }
            sphereCastDirection = target.position + Vector3.up - transform.position + (Vector3.up * 2.5f);

            //If in range Check LOS
            if (GetData("Patroling") == null || !(bool)GetData("Patroling"))
            {
                RaycastHit hit;
                if (Physics.SphereCast(transform.position + (Vector3.up * 2.5f), 1f, sphereCastDirection, out hit, distanceToTarget))
                {
                    if (!hit.transform.CompareTag("Player"))
                    {
                        SetData("Patroling", false);
                        state = NodeState.FAILURE;
                        return state;
                    }
                    state = NodeState.SUCCESS;
                    return state;
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
                    SetDistanceToTarget(Vector3.Distance(transform.position, target.position));
                }
                await Task.Delay(2);
            }
        }
    }
}
