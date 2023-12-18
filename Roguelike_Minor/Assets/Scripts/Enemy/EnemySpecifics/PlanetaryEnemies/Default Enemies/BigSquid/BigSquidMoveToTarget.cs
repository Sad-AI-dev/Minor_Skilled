using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Game.Enemy.Core;
using Game.Core;

namespace Game.Enemy {
    public class BigSquidMoveToTarget : BT_Node
    {
        //Components
        private Agent agent;
        private Transform transform;
        private Transform target;
        private Rigidbody rb;
        private NavMeshPath path;
        private Coroutine calculatePathCo;

        private float randomMinimumHeight = 0;
        private float rotationSpeedMoving;

        private Queue<Vector3> pathQueue = new Queue<Vector3>();

        public BigSquidMoveToTarget(Transform transform, Agent agent, Rigidbody rb, float rotationSpeedMoving)
        {
            this.transform = transform;
            this.agent = agent;
            this.rb = rb;
            this.rotationSpeedMoving = rotationSpeedMoving;
        }

        public override NodeState Evaluate()
        {
            if(randomMinimumHeight == 0) randomMinimumHeight = Random.Range(BigSquidTree.MinimumHeight, BigSquidTree.MinimumHeight + 10);
            if (target == null && GameStateManager.instance.player != null) target = GameStateManager.instance.player.transform;

            if (target != null)
            {
                //Check if not targeting
                if(GetData("Targeting") != null && (bool)GetData("Targeting"))
                {
                    state = NodeState.RUNNING;
                    return state;
                }

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

                        pathQueue.Enqueue(RandomPointBetweenVectors(lastCorner, nextCorner));
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

        Vector3 RandomPointBetweenVectors(Vector3 v1, Vector3 v2)
        {
            Vector3 middle = (v2 - v1) / 2 + v1;
            Vector3 RandomUnitVector = middle + Random.insideUnitSphere;
            return RandomUnitVector;
        }
    }
}
