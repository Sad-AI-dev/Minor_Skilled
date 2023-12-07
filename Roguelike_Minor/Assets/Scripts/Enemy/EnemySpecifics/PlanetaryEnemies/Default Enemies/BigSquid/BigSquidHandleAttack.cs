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
        LineRenderer lineRenderer;
        Rigidbody rb;
        float rotationSpeedTargeting;

        public BigSquidHandleAttack(Transform transform, Agent agent, LineRenderer lineRenderer, Rigidbody rb, float rotationSpeedTargeting)
        {
            this.agent = agent;
            this.lineRenderer = lineRenderer;
            this.rb = rb;
            this.transform = transform;
            this.rotationSpeedTargeting = rotationSpeedTargeting;
        }
        BigSquidPrimaryVars vars;
        public override NodeState Evaluate()
        {
            if (target == null) target = (Transform)GetData("Target");

            if(target != null)
            {
                //Handle Rotation
                HandleRotation();

                lineRenderer.SetPosition(0, agent.abilities.primary.originPoint.position);
                lineRenderer.SetPosition(1, agent.abilities.primary.originPoint.position + (agent.abilities.primary.originPoint.forward * 30));

                vars = agent.abilities.primary.vars as BigSquidPrimaryVars;
                vars.target = this.target;
                vars.lineRenderer = this.lineRenderer;
                
                agent.abilities.primary.TryUse();

                //TODO: Fly around, change in a bit 
                SetData("MoveDirection", (transform.right) * agent.stats.walkSpeed);
            }


            return state;
        }

        private void HandleRotation()
        {
            Vector3 dir = ((target.position + (Vector3.up/2)) - transform.position).normalized;

            Quaternion targetRotation = Quaternion.LookRotation(dir);
            targetRotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationSpeedTargeting * Time.deltaTime);

            SetData("MoveRotation", targetRotation);
        }
    }
}
