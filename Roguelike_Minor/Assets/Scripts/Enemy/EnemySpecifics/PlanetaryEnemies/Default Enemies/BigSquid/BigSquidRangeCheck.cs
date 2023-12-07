using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Game.Enemy.Core;
using Game.Core;
using System.Threading.Tasks;

namespace Game.Enemy {
    public class BigSquidRangeCheck : BT_Node
    {
        BigSquidPrimaryVars vars;

        public BigSquidRangeCheck(Transform transform, Agent agent)
        {
            this.transform = transform;
            this.agent = agent;
            EventBus<GameEndEvent>.AddListener(DoClearTarget);
        }
        
        public override NodeState Evaluate()
        {
            HandleSetTarget();
            if (GetData("DistanceToTarget") == null) CheckDistance();
            if (GetData("RandomFireRange") == null) SetData("RandomFireRange", Random.Range(BigSquidTree.FireRangeMin, BigSquidTree.FireRangeMax));

            if (GetData("DistanceToTarget") == null)
            {
                state = NodeState.FAILURE;
                return state;
            }
            if (vars == null) vars = agent.abilities.primary.vars as BigSquidPrimaryVars;
            if ((float)GetData("DistanceToTarget") > (int)GetData("RandomFireRange"))
            {
                state = NodeState.FAILURE;
                if (vars.targetingCo != null)
                {
                    agent.StopCoroutine(vars.targetingCo);
                    vars.targetingCo = null;
                }
                SetData("Targeting", false);

                return state;
            }
            else
            {
                Vector3 dir = ((target.position + (Vector3.up / 2)) - transform.position).normalized;

                RaycastHit hit;
                if (Physics.Raycast(transform.position, dir, out hit, Mathf.Infinity))
                {
                    if (hit.transform.CompareTag("Player"))
                    {
                        SetData("Targeting", true);
                        state = NodeState.SUCCESS;
                    }
                    else
                    {
                        if (GetData("Targeting") != null && (bool)GetData("Targeting"))
                        {
                            state = NodeState.SUCCESS;
                            return state;
                        }

                        state = NodeState.FAILURE;
                    }
                }
            }

            return state;
        }

        private void HandleSetTarget()
        {
            if (GetData("Target") == null) SetTarget(GameStateManager.instance.player.transform);
            target = (Transform)GetData("Target");
        }
        private async void CheckDistance()
        {
            while (transform != null && target != null)
            {
                if (target != null && transform != null)
                {
                    SetData("DistanceToTarget", Vector3.Distance(transform.position, target.position));
                }
                await Task.Delay(2);
            }
        }
        private void DoClearTarget(GameEndEvent eventData)
        {
            ClearData("Target");
            target = null;
            EventBus<GameEndEvent>.RemoveListener(DoClearTarget);
        }
    }
}

