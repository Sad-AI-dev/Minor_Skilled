using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Enemy.Core;
using System.Threading.Tasks;

namespace Game.Enemy
{
    public class TaskCheckSemi : BT_Node
    {
        Agent agent;
        Coroutine getRandomCo;

        bool ranged;
        int chargeChancePercent;

        int random;
        bool checkingRandom;

        public TaskCheckSemi(bool ranged, int chargeChancePercent, Agent agent)
        {
            this.ranged = ranged;
            this.chargeChancePercent = chargeChancePercent;
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
                if ((float)GetData("DistanceToTarget") <= MeleeGruntTree.semiMeleeAttackRange && 
                    (float)GetData("DistanceToTarget") > MeleeGruntTree.meleeAttackRange)
                {
                    //Get random number.
                    if (GetData("getRandomCO") == null) parent.parent.SetData("getRandomCO", agent.StartCoroutine(GetRandomCO()));

                    //Fail charge if true and keep walking;
                    if ((int)GetData("ChargeRandom") <= chargeChancePercent)
                    {
                        parent.parent.SetData("Charging", true);
                        Charged();
                        QuickSetData("ChargeRandom", chargeChancePercent + 1);
                        agent.StopCoroutine((Coroutine)GetData("getRandomCO"));
                        state = NodeState.SUCCESS;
                        return state;
                    }
                    else
                    {
                        state = NodeState.FAILURE;
                        return state;
                    }
                }
                else state = NodeState.FAILURE;
            }

            return state;
        }

        async void Charged()
        {
            await Task.Delay(5000);
            parent.parent.SetData("Charging", false);
        }

        IEnumerator GetRandomCO()
        {
            if (GetData("ChargeRandom") != null && (int)GetData("ChargeRandom") <= chargeChancePercent)
            {
                QuickSetData("ChargeRandom", chargeChancePercent + 1);
            }
            else QuickSetData("ChargeRandom", Random.Range(1, 101));
            checkingRandom = true;
            yield return new WaitForSeconds(1);
            parent.parent.SetData("getRandomCO", agent.StartCoroutine(GetRandomCO()));
        }

        void QuickSetData(string name, object value)
        {
            parent.parent.SetData(name, value);
        }
        async void GetRandom()
        {
            if(random <= chargeChancePercent)
            {
                random = chargeChancePercent + 1;
            }
            else random = Random.Range(1, 101);
            checkingRandom = true;
            await Task.Delay(1000);
            GetRandom();
        }
    }
}
