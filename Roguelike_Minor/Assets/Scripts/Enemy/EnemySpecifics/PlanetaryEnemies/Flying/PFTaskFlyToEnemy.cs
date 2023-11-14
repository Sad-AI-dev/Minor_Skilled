using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Enemy.Core;
using Game.Core.Data;
using Game.Enemy.Pathfinding;
using System.Threading.Tasks;

namespace Game.Enemy {
    public class PFTaskFlyToEnemy : BT_Node
    {
        private Transform transform;
        private Transform target;
        private GNode targetNode;

        //Pathfinding
        private Vector3 currentNode;
        private Vector3 currentTarget;
        private Vector3 targetNodePosOld;

        public PFTaskFlyToEnemy(Transform transform)
        {
            this.transform = transform;

            //set current node on spawn
            SetCurrentNode();
        }

        public override NodeState Evaluate()
        {
            //Get target
            if (GetData("Target") == null) parent.SetData("Target", GameStateManager.instance.player.transform);
            target = (Transform)GetData("Target");

            //Get node closest to player
            if (GetData("TargetNode") == null) SetTargetNode();
            GNode targetNode = (GNode)GetData("TargetNode");
            Vector3 targetNodePos = new Vector3(targetNode.x, targetNode.y, targetNode.z);

            //Get Path
            if (targetNodePos != targetNodePosOld)
            {
                PFTree.path = new Queue<Vector3>(GridBuilder.GetInstance().GetPath(currentNode, targetNodePos, 0));
                targetNodePosOld = targetNodePos;
            }

            //Move
            MoveOverPath();
            state = NodeState.RUNNING;

            return state;
        }

        void MoveOverPath()
        {
            if (PFTree.path != null && currentTarget != PFTree.path.Peek())
            {
                //Debug.Log("Getting target");
                currentTarget = PFTree.path.Peek();
            }
            if (PFTree.path.Count > 0)
            {
               //Make move thru spine
               transform.position = //move
                    Vector3.MoveTowards(
                        transform.position, //from
                        currentTarget, //to
                        PFTree.FlightSpeed * Time.deltaTime); //speed

                //transform.Translate((currentTarget - transform.position) * PFTree.FlightSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, PFTree.path.Peek()) < 0.01)
                {
                    //Debug.Log("Target Reached");
                    transform.position = currentTarget;
                    SetCurrentNode();
                    PFTree.path.Dequeue();
                    if (PFTree.path.Count > 0) currentTarget = PFTree.path.Peek();
                }
            }
        }

        private void SetCurrentNode() 
        {
            GNode temp = GridBuilder.GetInstance().GetNodeClosestToWorldPos(transform.position);
            //Debug.Log(temp.worldPosition);
            currentNode = new Vector3(temp.x, temp.y, temp.z);
        }

        private async void SetTargetNode()
        {
            while (transform != null)
            {
                if (target != null)
                {
                    parent.SetData("TargetNode", GridBuilder.GetInstance().GetNodeClosestToWorldPos(target.position));
                }
                else
                {
                    //Debug.Log("no target");
                }
                await Task.Delay(2);
            }
        }
    }
}
