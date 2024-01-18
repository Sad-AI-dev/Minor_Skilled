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
        float rotationSpeedTargeting;
        public BigSquidRangeCheck(Transform transform, float distanceToCheck, Agent agent, float rotationSpeedTargeting) : base(agent, distanceToCheck)
        {
            this.transform = transform;
            this.agent = agent;
            this.rotationSpeedTargeting = rotationSpeedTargeting;
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
                        HandleTargetingMovementAndRotation();
                        return state;
                    }
                    else
                    {
                        if (GetData("Targeting") != null && (bool)GetData("Targeting"))
                        {
                            state = NodeState.SUCCESS;
                            HandleTargetingMovementAndRotation();
                            return state;
                        }

                        state = NodeState.FAILURE;
                    }
                }
            }
            
            return state;
        }

        void HandleTargetingMovementAndRotation()
        {
            //Handle Rotation
            HandleRotation();
            SetData("MoveDirection", (transform.right) * agent.stats.walkSpeed);
        }
        private void HandleRotation()
        {
            Vector3 dir = ((target.position + -Vector3.up / 2) - transform.position).normalized;

            Quaternion targetRotation = Quaternion.LookRotation(dir);
            targetRotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationSpeedTargeting * Time.deltaTime);

            SetData("MoveRotation", targetRotation);
        }
    }
}

