using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Enemy.Core;
using UnityEngine.AI;

namespace Game.Enemy {
    public class TaskCheckMelee: CheckRangeNode
    {
        bool ranged;
        bool charging = false;

        public TaskCheckMelee(Transform transform, float distanceToCheck, bool ranged, Agent agent) : base(transform, distanceToCheck)
        {
            this.ranged = ranged;
            this.agent = agent;
        }

        public override NodeState Evaluate()
        {
            if ((bool)GetData("Ranged") != ranged)
            {
                state = NodeState.FAILURE;
                return state;
            }
            if (GetData("Charging") != null)
            {
                charging = (bool)GetData("Charging");
            }
            state = base.Evaluate();

            if(state == NodeState.SUCCESS)
            {
                if (!charging)
                {
                    //Stop charging co routine
                    if (GetData("getRandomCO") != null)
                    {
                        agent.StopCoroutine((Coroutine)GetData("getRandomCO"));
                        SetData("getRandomCO", null);
                    }
                }
            }

            return state;
        }
    }
}
