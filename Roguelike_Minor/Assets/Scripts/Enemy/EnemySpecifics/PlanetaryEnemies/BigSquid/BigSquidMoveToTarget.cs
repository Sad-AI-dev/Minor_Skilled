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

        private Queue<Vector3> pathQueue = new Queue<Vector3>();

        public BigSquidMoveToTarget(Transform transform, Agent agent, Rigidbody rb)
        {
            this.transform = transform;
            this.agent = agent;
            this.rb = rb;
        }

        public override NodeState Evaluate()
        {
            if(randomMinimumHeight == 0) randomMinimumHeight = Random.Range(BigSquidTree.MinimumHeight, BigSquidTree.MinimumHeight + 25);
            if (target == null && GameStateManager.instance.player != null) target = GameStateManager.instance.player.transform;

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

                    //Move
                    rb.MovePosition(transform.position + dir * (agent.stats.walkSpeed * Time.deltaTime));
                    rb.MoveRotation(targetRotation);

                    //Check for target range
                    float distance = Vector3.Distance(transform.position, pathQueue.Peek());
                    if (distance <= 0.1f)
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
            Vector3 origin = transform.position;
            origin.y = 0;
            NavMesh.CalculatePath(origin, target.position, NavMesh.AllAreas, path);

            pathQueue.Clear();

            if (path.corners != null)
            {
                if (path.corners.Length > 0)
                {
                    for (int i = 1; i < path.corners.Length - 1; i++)
                    {
                        Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red, 100);
                        Vector3 corner = path.corners[i];
                        //ToDo, Change to random height;
                        corner.y += randomMinimumHeight;
                        pathQueue.Enqueue(corner);
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
