using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Game.Enemy.Core;
using Game.Core;
using System.Threading.Tasks;

namespace Game.Enemy {
    public class Rock_BigSquid_HandleRangeCheckRanged : CheckRangeNode
    {
        float rotationSpeedTargeting;
        Vector3 dir;
        public Rock_BigSquid_HandleRangeCheckRanged(Transform transform, float distanceToCheck, Agent agent, float rotationSpeedTargeting, NavMeshAgent navAgent) : base(agent, distanceToCheck)
        {
            this.transform = transform;
            this.agent = agent;
            this.rotationSpeedTargeting = rotationSpeedTargeting;
            this.navAgent = navAgent;
        }

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
                        HandleRotation();
                        return state;
                    }
                    else
                    {
                        if (GetData("Targeting") != null && (bool)GetData("Targeting"))
                        {
                            HandleRotation();
                            return state;
                        }
                        navAgent.updatePosition = true;
                        state = NodeState.FAILURE;
                    }
                }
            }
            else
            {
                navAgent.updatePosition = true;
            }

            return state;
        }

        private void HandleRotation()
        {
            /*Vector3 dir = ((target.position) - transform.position).normalized;

            Quaternion targetRotation = Quaternion.LookRotation(dir);
            targetRotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationSpeedTargeting * Time.deltaTime);

            SetData("MoveRotation", targetRotation);*/

            navAgent.SetDestination(target.position);
            navAgent.updatePosition = false;

            //navAgent.isStopped = true;
        }
    }
}
