using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Game.Enemy.Core;
using Game.Core;

namespace Game.Enemy {
    public class Rock_BigSquid_HandleRanged : BT_Node
    {
        float rotationSpeedTargeting;

        public Rock_BigSquid_HandleRanged(Transform transform, Agent agent, NavMeshAgent navAgent, float rotationSpeedTargeting)
        {
            this.agent = agent;
            this.transform = transform;
            this.rotationSpeedTargeting = rotationSpeedTargeting;
            this.navAgent = navAgent;
        }

        public override NodeState Evaluate()
        {
            state = NodeState.RUNNING;

            if (target == null) target = (Transform)GetData("Target");

            if (target != null)
            {
                navAgent.isStopped = true;
                transform.LookAt(target);

                agent.abilities.primary.TryUse();
            }

            return state;
        }

        private void HandleRotation()
        {
            Vector3 dir = ((target.position + (Vector3.up / 3)) - transform.position).normalized;

            Quaternion targetRotation = Quaternion.LookRotation(dir);
            targetRotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationSpeedTargeting * Time.deltaTime);

            SetData("MoveRotation", targetRotation);
        }
    }
}
