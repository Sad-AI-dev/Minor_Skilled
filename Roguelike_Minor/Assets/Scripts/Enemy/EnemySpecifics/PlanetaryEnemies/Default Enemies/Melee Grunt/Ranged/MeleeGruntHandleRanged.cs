using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Enemy.Core;
using System.Threading.Tasks;
using UnityEngine.AI;
using Game.Core;

namespace Game.Enemy {
    public class MeleeGruntHandleRanged : BT_Node
    {
        /*Transform A, B, Control;*/
        Transform target;
        Transform transform;
        NavMeshAgent navAgent;
        Agent agent;

        float distanceToTarget, rangedAttackRange;

        public MeleeGruntHandleRanged(Transform transform, float rangedAttackRange, Agent agent, NavMeshAgent navAgent)
        {
            this.transform = transform;
            this.rangedAttackRange = rangedAttackRange;
            this.agent = agent;
            this.navAgent = navAgent;
        }

        public override NodeState Evaluate()
        {
            //----=====Get Variables=====----
            //Get Target
            if (GetData("Target") != null) 
            {
                target = (Transform)GetData("Target");
            }

            //Get distance to target;
            if (GetData("DistanceToTarget") == null) CheckDistance();
            distanceToTarget = (float)GetData("DistanceToTarget");

            //----=====LOGIC=====----
            //If target out of range: Fail
            if (distanceToTarget > rangedAttackRange)
            {
                state = NodeState.FAILURE;
            }
            //Else If target in range and 3 or more enemies next to player: Succeed
            else
            {
                if (target != null && transform != null)
                {
                    //Stop moving
                    navAgent.velocity = Vector3.zero;

                    //Rotate to target
                    Vector3 targetPostition = new Vector3(target.position.x, transform.position.y, target.position.z);
                    transform.LookAt(targetPostition);
                    agent.abilities.secondary.TryUse();
                }
                state = NodeState.RUNNING;
            }

            return state;
        }

        private async void CheckDistance()
        {
            while (transform != null && target )
            {
                if (target != null && transform != null)
                {
                    SetData("DistanceToTarget", Vector3.Distance(transform.position, target.position));
                }
                await Task.Delay(2);
            }
        }
       /* public Vector3 EvalBezier(float t)
        {
            Vector3 ac = Vector3.Lerp(A.position, Control.position, t);
            Vector3 cb = Vector3.Lerp(Control.position, B.position, t);

            return Vector3.Lerp(ac, cb, t);
        }*/
    }
}
