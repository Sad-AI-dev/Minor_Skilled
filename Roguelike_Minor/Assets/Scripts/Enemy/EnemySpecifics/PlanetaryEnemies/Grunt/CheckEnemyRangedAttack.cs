using UnityEngine;
using Game.Enemy.Core;
using System.Threading.Tasks;

namespace Game.Enemy {
    public class CheckEnemyRangedAttack : BT_Node
    {
        private Transform transform;
        private Transform target;

        private float distanceToTarget;
        private LayerMask LOSLayerMask;

        public CheckEnemyRangedAttack(Transform transform, LayerMask LOSLayerMask)
        {
            this.transform = transform;
            this.LOSLayerMask = LOSLayerMask;
        }

        public override NodeState Evaluate()
        {
            //----=====Get Variables=====----
            //Get Target
            if ((Transform)GetData("Target") == null) parent.SetData("Target", GameStateManager.instance.player.transform);
            target = (Transform)GetData("Target");

            //Get distance to target;
            if (GetData("DistanceToTarget") == null) CheckDistance();
            distanceToTarget = (float)GetData("DistanceToTarget");

            //----=====LOGIC=====----
            //If target out of range: Fail
            if (distanceToTarget > PGTree.rangedAttackRange)
            {
                state = NodeState.FAILURE;
            }
            //Else If target in range but no melee attackers: Fail
            else if(PGTree.EnemiesInRangeOfPlayer < 3)
            { 
                state = NodeState.FAILURE;
            }
            //Else if not in LOS: Fail
            else if(NotInLOS())
            {
                state = NodeState.FAILURE;
            }
            //Else If target in range and 3 or more enemies next to player: Succeed
            else
            {
                state = NodeState.SUCCESS;
            }

            return state;
        }

        private bool NotInLOS()
        {
            bool notInLOS = false;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, target.position + Vector3.up - transform.position, out hit, Mathf.Infinity, LOSLayerMask))
            {
                if (hit.transform.tag == "Player")
                {
                    notInLOS = false;
                }
                else
                {
                    notInLOS = true;
                }
            }
            return notInLOS;
        }
        private async void CheckDistance()
        {
            while (transform != null)
            {
                if (target != null && transform != null)
                {
                    parent.parent.SetData("DistanceToTarget", Vector3.Distance(transform.position, target.position));
                }
                await Task.Delay(2);
            }
        }
    }
}
