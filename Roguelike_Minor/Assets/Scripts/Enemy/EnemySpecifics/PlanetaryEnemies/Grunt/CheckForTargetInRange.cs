using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Enemy.Core;
using UnityEngine.AI;

namespace Game.Enemy
{
    public class CheckForTargetInRange : BT_Node
    {
        private Transform transform;
        private LayerMask playerLayerMask;

        public CheckForTargetInRange(Transform transform, LayerMask layermask)
        {
            this.transform = transform;
            playerLayerMask = layermask;
        }

        public override NodeState Evaluate()
        {
            object t = GetData("Target");

            if (t == null)
            {
                Collider[] col = Physics.OverlapSphere(
                    transform.position, PGTree.FOVRange, playerLayerMask);

                if (col.Length > 0)
                {
                    parent.parent.SetData("Target", col[0].transform);
                    Debug.Log("Target Found");
                    state = NodeState.SUCCESS;
                    return state;
                }
                state = NodeState.FAILURE;
                return state;
            }
            state = NodeState.SUCCESS;
            return state;
        }
    }
}