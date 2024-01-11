using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Enemy.Core;
using System.Threading.Tasks;

namespace Game.Enemy {
    public class TaskCheckSemi : CheckRangeNode
    {
        bool ranged;
        int chargeChancePercent;

        public TaskCheckSemi(Transform transform, float distanceToCheck, bool ranged, int chargeChancePercent, Agent agent) : base(agent, distanceToCheck)
        {
            this.ranged = ranged;
            this.chargeChancePercent = chargeChancePercent;
            this.agent = agent;
        }

        int chanceNumber;

        public override NodeState Evaluate()
        {
            if ((bool)GetData("Ranged") != ranged)
            {
                state = NodeState.FAILURE;
                return state;
            }
            state = base.Evaluate();

            if(state == NodeState.SUCCESS) 
            {
                state = NodeState.FAILURE;
                if ((float)GetData("DistanceToTarget") > MeleeGruntTree.meleeAttackRange)
                {
                    //Chance to charge
                    if(null == GetData("getRandomCO")) SetData("getRandomCO", agent.StartCoroutine(CheckForChargeCooldownCo()));

                    if(chanceNumber <= chargeChancePercent)
                    {
                        agent.StartCoroutine(ChargingCo());
                        state = NodeState.SUCCESS;
                    }
                }
            }

            return state;
        }

        IEnumerator ChargingCo()
        {
            SetData("Charging", true);
            yield return new WaitForSeconds(5);
            ClearData("getRandomCO");
            SetData("Charging", false);
        }

        IEnumerator CheckForChargeCooldownCo()
        {
            chanceNumber = Random.Range(1, 101);
            Debug.Log("Chance Charge: " + chanceNumber);
            yield return new WaitForSeconds(3);
            if (null == GetData("Charging") || !(bool)GetData("Charging"))
            {
                SetData("getRandomCO", agent.StartCoroutine(CheckForChargeCooldownCo()));
            }
        }
    }
}
