using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Enemy.Core;
using UnityEngine.AI;
using Game.Core;

namespace Game.Enemy {
    public class MoveToTargetFlyingNode : BT_Node
    {
        //Components
        private Rigidbody rb;
        private NavMeshPath path;
        private Coroutine calculatePathCo;

        private float randomMinimumHeight = 0;
        private float rotationSpeedMoving;

        private Queue<Vector3> pathQueue = new Queue<Vector3>();

        public MoveToTargetFlyingNode(Transform transform, Agent agent, Rigidbody rb, float rotationSpeedMoving, float randomMinimumHeight)
        {
            this.transform = transform;
            this.agent = agent;
            this.rb = rb;
            this.rotationSpeedMoving = rotationSpeedMoving;
            this.randomMinimumHeight = randomMinimumHeight;
        }

        public override NodeState Evaluate()
        {
            if (GetData("Target") == null) SetTarget(GameStateManager.instance.player.transform);
            target = (Transform)GetData("Target");

            if (target != null)
            {
                //Check if we have a path
                if (calculatePathCo == null)
                {
                    calculatePathCo = agent.StartCoroutine(CalculatePathCO());
                }

                //follow path
                if (pathQueue.Count > 0)
                {
                    //Handle Direction
                    Vector3 dir = (pathQueue.Peek() - transform.position).normalized;

                    //Handle rotation
                    Quaternion targetRotation = Quaternion.LookRotation(dir);
                    targetRotation = Quaternion.RotateTowards(
                        transform.rotation,
                        targetRotation,
                        rotationSpeedMoving * Time.deltaTime);

                    //Move
                    SetData("MoveDirection", dir * agent.stats.sprintSpeed);
                    SetData("MoveRotation", targetRotation);

                    //Check for target range
                    float distance = Vector3.Distance(transform.position, pathQueue.Peek());
                    if (distance <= 0.2f)
                    {
                        pathQueue.Dequeue();
                    }
                }
                //If not path, stand still
                else rb.velocity = Vector3.zero;
            }
            state = NodeState.RUNNING;
            return state;
        }

        IEnumerator CalculatePathCO()
        {
            path = new NavMeshPath();
            Vector3 origin = transform.position;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -transform.up, out hit, Mathf.Infinity))
            {
                origin.y = hit.point.y;
            }
            NavMesh.CalculatePath(origin, target.position, NavMesh.GetAreaFromName("BigSquid"), path);

            pathQueue.Clear();

            if (path.corners != null)
            {
                if (path.corners.Length > 0)
                {
                    for (int i = 1; i < path.corners.Length - 1; i++)
                    {
                        Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red, 100);

                        Vector3 nextCorner = path.corners[i];
                        nextCorner.y += randomMinimumHeight;

                        Vector3 lastCorner = path.corners[i - 1];
                        lastCorner.y += randomMinimumHeight;

                        pathQueue.Enqueue(nextCorner);
                    }
                    Vector3 targetpos = target.position;
                    //ToDo, Change to random height;
                    targetpos.y += randomMinimumHeight;
                    pathQueue.Enqueue(targetpos);
                }
            }
            yield return new WaitForSeconds(2);
            calculatePathCo = agent.StartCoroutine(CalculatePathCO());
        }

    }
}
