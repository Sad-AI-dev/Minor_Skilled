using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Game.Enemy.Core;
using Game.Core;
using System.Threading.Tasks;

namespace Game.Enemy {
    public class BigSquidRangeCheck : CheckRangeNode
    {
        public BigSquidRangeCheck(Transform transform, float distanceToCheck, Agent agent) : base(transform, distanceToCheck)
        {
            this.agent = agent;
        }

        Vector3 dir;
        public override NodeState Evaluate()
        {
            state = base.Evaluate();

            if (state == NodeState.SUCCESS)
            {
                dir = ((target.position + (Vector3.up / 2)) - transform.position).normalized;

                RaycastHit hit;
                if (Physics.Raycast(transform.position, dir, out hit, Mathf.Infinity))
                {
                    if (hit.transform.CompareTag("Player"))
                    {
                        SetData("Targeting", true);
                        return state;
                    }
                    else
                    {
                        if (GetData("Targeting") != null && (bool)GetData("Targeting"))
                        {
                            state = NodeState.SUCCESS;
                            return state;
                        }

                        state = NodeState.FAILURE;
                    }
                }
            }
            
            return state;
        }
    }
}

