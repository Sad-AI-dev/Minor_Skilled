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
        Transform transform;
        Transform target;
        Agent agent;
        LineRenderer lineRenderer;
        Rigidbody rb;

        public BigSquidHandleAttack(Transform transform, Agent agent, LineRenderer lineRenderer, Rigidbody rb)
        {
            this.agent = agent;
            this.lineRenderer = lineRenderer;
            this.rb = rb;
            this.transform = transform;
        }

        public override NodeState Evaluate()
        {
            if (target == null) target = (Transform)GetData("Target");
            if(target != null) transform.LookAt(target);
            lineRenderer.SetPosition(0, agent.abilities.primary.originPoint.position);
            lineRenderer.SetPosition(1, target.position + Vector3.up);
            agent.abilities.primary.vars = new BigSquidPrimaryVars
            {
                target = this.target,
                lineRenderer = this.lineRenderer
            };
            agent.abilities.primary.TryUse();
            parent.parent.ClearData("MoveDirection");
            rb.velocity = Vector3.zero;

            return state;
        }
    }
}
