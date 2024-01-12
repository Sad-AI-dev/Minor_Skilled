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
        Vector3 dir;
        public Rock_BigSquid_HandleRangeCheckRanged(Transform transform, float distanceToCheck, Agent agent, NavMeshAgent navAgent) : base(agent, distanceToCheck)
        {
            this.transform = transform;
            this.agent = agent;
            this.navAgent = navAgent;
        }

        public override NodeState Evaluate()
        {
            state = base.Evaluate();

            if (state == NodeState.SUCCESS)
            {
                dir = ((target.position + (Vector3.up / 2)) - transform.position).normalized;
                HandleRotation();
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
                            return state;
                        }
                        state = NodeState.FAILURE;
                    }
                }
            }

            return state;
        }

        Vector3 rotDir;
        Quaternion targetRotation;
        float angle;
        float r;
        private void HandleRotation()
        {
            rotDir = ((target.position) - transform.position).normalized;

            targetRotation = Quaternion.LookRotation(rotDir);
            targetRotation.x = 0;
            targetRotation.z = 0;

            SetData("MoveRotation", targetRotation);

            angle = Quaternion.Angle(Quaternion.LookRotation(transform.forward), Quaternion.LookRotation(dir));
            SetData("RotationLeft", angle);

            navAgent.isStopped = true;
        }
    }
}
