using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Game.Enemy.Core;
using Game.Core;

namespace Game.Enemy {
    public class Boss_BigSquid_MoveToTarget : BT_Node
    { 
        //Components
        private Agent agent;
        private Transform transform;
        private Transform target;
        private Rigidbody rb;
        private NavMeshPath path;
        private Coroutine calculatePathCo;

        private float randomMinimumHeight = 0;

        private Queue<Vector3> pathQueue = new Queue<Vector3>();

        public Boss_BigSquid_MoveToTarget(Transform transform, Agent agent, Rigidbody rb)
        {
            this.transform = transform;
            this.agent = agent;
            this.rb = rb;
        }
        public override NodeState Evaluate()
        {
            if (randomMinimumHeight == 0) randomMinimumHeight = Random.Range(Boss_BigSquidTree.MinimumHeight, Boss_BigSquidTree.MinimumHeight + 20);
            if (GetData("Target") != null) target = (Transform)GetData("Target");
            if (target != null)
            {
                if (calculatePathCo == null)
                {
                    calculatePathCo = agent.StartCoroutine(CalculatePathCO());
                }

                if (pathQueue.Count > 0)
                {
                    //Handle Direction
                    Vector3 dir = (pathQueue.Peek() - transform.position).normalized;

                    //Handle rotation
                    Quaternion targetRotation = Quaternion.LookRotation(dir);
                    targetRotation = Quaternion.RotateTowards(
                        transform.rotation,
                        targetRotation,
                        360 * Time.deltaTime);
                    targetRotation.x = 0;
                    targetRotation.z = 0;
                    //Move
                    if (GetData("Patroling") == null || !(bool)GetData("Patroling"))
                    {
                        SetData("MoveDirection", dir * agent.stats.sprintSpeed);
                        SetData("TargetRotation", targetRotation);
                    }

                    //Check for target range
                    float distance = Vector3.Distance(transform.position, pathQueue.Peek());
                    if (distance <= 0.2f)
                    {
                        pathQueue.Dequeue();
                    }
                }
                else rb.velocity = Vector3.zero;
            }
            state = NodeState.RUNNING;
            return state;
        }
        IEnumerator CalculatePathCO()
        {
            path = new NavMeshPath();

            //Check position on NavMesh Surface
            Vector3 origin = transform.position;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -transform.up, out hit, Mathf.Infinity))
            {
                origin.y = hit.point.y;
            }
            NavMesh.CalculatePath(origin, target.position, NavMesh.GetAreaFromName("Boss_BigSquid"), path);

            pathQueue.Clear();

            if (path.corners != null)
            {
                if (path.corners.Length > 0)
                {
                    for (int i = 1; i < path.corners.Length - 1; i++)
                    {
                        Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red, 100);

                        pathQueue.Enqueue(path.corners[i]);
                    }
                    Vector3 targetpos = target.position;
                    targetpos.y += randomMinimumHeight;
                    pathQueue.Enqueue(targetpos);
                }
            }
            yield return new WaitForSeconds(1);
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
