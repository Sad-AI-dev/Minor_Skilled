using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Enemy.Core;
using UnityEngine.AI;

namespace Game.Enemy {
    public class TaskCheckMelee: BT_Node
    {
        bool ranged;
        bool charging = false;
        Agent agent;

        public TaskCheckMelee(bool ranged, Agent agent)
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
            else
            {
                if (GetData("Charging") != null)
                {
                    charging = (bool)GetData("Charging");
                    Debug.Log((bool)GetData("Charging"));
                }
                if ((float)GetData("DistanceToTarget") <= MeleeGruntTree.meleeAttackRange && !charging)
                {
                    if (GetData("ChargeRandom") != null)
                    {
                        parent.parent.SetData("ChargeRandom", 100);
                    }
                    if (GetData("getRandomCO") != null)
                    {
                        agent.StopCoroutine((Coroutine)GetData("getRandomCO"));
                        parent.parent.SetData("getRandomCO", null);
                    }
                    Debug.Log("attacking  Melee");
                    state = NodeState.SUCCESS;
                }
                else
                {
                    state = NodeState.FAILURE;
                }
            }

            return state;
        }
    }
}
