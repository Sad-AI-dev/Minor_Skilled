using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using Game.Core;
using Game.Enemy.Core;

namespace Game.Enemy {
    public class BigSquidHandleAttack : BT_Node
    {
        Transform lazerOrigin;
        Transform target;
        Agent agent;
        LineRenderer lineRenderer;

        Coroutine targetingCo;

        public BigSquidHandleAttack(Agent agent, LineRenderer lineRenderer, Transform laserOrigin)
        {
            this.agent = agent;
            this.lineRenderer = lineRenderer;
            this.lazerOrigin = laserOrigin;
        }

        public override NodeState Evaluate()
        {
            if (target == null) target = (Transform)GetData("Target");
            if (targetingCo == null) targetingCo = agent.StartCoroutine(TargetingCo());

            return state;
        }

        IEnumerator TargetingCo()
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, lazerOrigin.position);
            lineRenderer.SetPosition(1, target.position + Vector3.up);
            yield return new WaitForSeconds(3);
            lineRenderer.enabled = false;
            agent.abilities.primary.TryUse();
        }
    }
}
