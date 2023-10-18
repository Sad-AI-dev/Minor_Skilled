using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Enemy.Core;
using System.Threading.Tasks;

namespace Game.Enemy {
    public class PFCheckEnemyInRange : BT_Node
    {
        private Transform transform;
        private Transform target;

        private float distanceToTarget;

        public PFCheckEnemyInRange(Transform transform)
        {
            this.transform = transform;
        }

        public override NodeState Evaluate()
        {
            //Get Target
            if (GetData("Target") == null) parent.SetData("Target", GameStateManager.instance.player.transform);
            target = (Transform)GetData("Target");

            //Get distance
            if (GetData("DistanceToTarget") == null) CheckDistance();
            distanceToTarget = (float)GetData("DistanceToTarget");

            //If enemy out of range - Fail
            if(distanceToTarget > PFTree.AttackRange)
            {
                state = NodeState.FAILURE;
            }
            //If enemy in range - succees
            if(distanceToTarget <= PFTree.AttackRange)
            {
                state = NodeState.SUCCESS;
            }

            return state;
        }

        private async void CheckDistance()
        {
            while (transform != null)
            {
                if (target != null)
                {
                    parent.SetData("DistanceToTarget", Vector3.Distance(transform.position, target.position));
                }
                else
                {
                    Debug.Log("no target");
                }
                await Task.Delay(2);
            }
        }
    }
}
