using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Enemy.Core;
using Game.Core;

namespace Game.Enemy {
    public class SmallSquidMoveToTarget : BT_Node
    {
        Transform transform;
        Transform target;
        Agent agent;
        LayerMask layerMask;
        GameObject ExplosionVisuals;
        Rigidbody rb;

        Coroutine explode;

        public SmallSquidMoveToTarget(Transform transform, Agent agent, LayerMask playerLayerMask, GameObject ExplosionVisuals, Rigidbody rb)
        {
            this.transform = transform;
            this.agent = agent;
            this.layerMask = playerLayerMask;
            this.ExplosionVisuals = ExplosionVisuals;
            this.rb = rb;
        }

        public override NodeState Evaluate()
        {
            target = (Transform)GetData("Target");

            if (target != null && transform != null)
            {
                Vector3 direction = ((target.position + Vector3.up * 1.5f) - transform.position).normalized;

                Collider[] col = Physics.OverlapSphere(transform.position, SmallSquidTree.ExplosionRange, layerMask);

                if (col.Length > 0 && explode == null)
                {
                    explode = agent.StartCoroutine(ExlodeAfterSeconds());
                    Debug.Log("Exploding");
                }
                else
                {
                    //Handle Direction
                    Vector3 dir = ((target.position + Vector3.up) - transform.position).normalized;

                    //Handle rotation
                    Quaternion targetRotation = Quaternion.LookRotation(dir);
                    targetRotation = Quaternion.RotateTowards(
                        transform.rotation,
                        targetRotation,
                        360 * Time.deltaTime);

                    //Move
                    rb.MovePosition(transform.position + dir * (agent.stats.walkSpeed * Time.deltaTime));
                    rb.MoveRotation(targetRotation);
                    //transform.Translate(direction * (Time.deltaTime * agent.stats.sprintSpeed));
                }
            }
            return state;
        }

        IEnumerator ExlodeAfterSeconds()
        {
            yield return new WaitForSeconds(1.5f);
            ExplosionVisuals.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            agent.abilities.primary.TryUse();
        }
    }
}
