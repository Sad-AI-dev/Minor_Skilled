using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Enemy.Core;
using Game.Core;
using System.Threading.Tasks;

namespace Game.Enemy {
    public class SmallSquidCheckRange : CheckRangeNode
    {
        public SmallSquidCheckRange(Transform transform, float distanceToCheck, Agent agent) : base(transform, distanceToCheck)
        {
            this.agent = agent;
        }

        Vector3 direction;
        public override NodeState Evaluate()
        {
            state = base.Evaluate();

            direction = ((target.position + Vector3.up ) - transform.position).normalized;

            if (state == NodeState.FAILURE)
            {
                if (GetData("Chasing") != null && (bool)GetData("Chasing")) SetData("Chasing", false);
                return state;
            }
            else
            {
                RaycastHit hit;
                if (Physics.SphereCast(transform.position, 0.5f, direction, out hit, (float)GetData("DistanceToTarget")))
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

            return state;
        }
    }
}
