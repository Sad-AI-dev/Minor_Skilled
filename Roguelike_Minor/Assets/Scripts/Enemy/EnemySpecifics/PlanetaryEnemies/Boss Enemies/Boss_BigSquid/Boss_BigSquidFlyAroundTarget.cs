using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Game.Enemy.Core;
using Game.Core;

namespace Game.Enemy {
    public class Boss_BigSquidFlyAroundTarget : BT_Node
    {
        Vector3 randomMovePosition = Vector3.zero;
        Transform target;
        Transform transform;
        Agent agent;

        public Boss_BigSquidFlyAroundTarget(Agent agent, Transform transform)
        {
            this.agent = agent;
            this.transform = transform;
        }

        public override NodeState Evaluate()
        {
            state = NodeState.SUCCESS;
            //Get random spot in unit circle around target.
            if (GetData("Target") != null) target = (Transform)GetData("Target");

            if (target == null)
            {
                state = NodeState.FAILURE;
                return state;
            }

            if (randomMovePosition == Vector3.zero)
            {
                Vector3 unitCirle = Random.insideUnitSphere * Boss_BigSquidTree.RandomMoveArea;
                unitCirle.y *= 0.2f;
                randomMovePosition = 
                    (target.position + (Vector3.up * Boss_BigSquidTree.MinimumHeight)) +
                    unitCirle;
            }
            //Check of position chosen is valid
            Vector3 dir = (randomMovePosition - transform.position).normalized;
            float distance = Vector3.Distance(transform.position, randomMovePosition);


            RaycastHit hit;
            if (Physics.SphereCast(transform.position + (Vector3.up * 2.5f), 1f, dir, out hit, distance))
            {
                //Invalid position
                randomMovePosition = Vector3.zero;
                return state;
            }
            

            //Fly to this random position.
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            targetRotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                360 * Time.deltaTime);
            targetRotation.x = 0;
            targetRotation.z = 0;

            SetData("Patroling", true);
            SetData("DistanceToCurrentTarget", distance);
            SetData("MoveDirection", dir * agent.stats.walkSpeed);
            SetData("TargetRotation", targetRotation);

            //Choose new position when reached the target
            if (distance <= 0.2f)
            {
                randomMovePosition = Vector3.zero;
            }

            return state;
        }
    }
}
