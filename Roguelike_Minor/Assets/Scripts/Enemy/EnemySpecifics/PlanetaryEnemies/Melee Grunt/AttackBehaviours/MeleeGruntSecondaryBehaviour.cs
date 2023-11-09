using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game.Enemy {
    public class MeleeGruntSecondaryBehaviour : Projectile
    {
        [HideInInspector] public Vector3 target;
        [HideInInspector] public Vector3 sourceTransform;
        [HideInInspector] public float bulletSpeed;
        [HideInInspector] public float sampleTime;
        Vector3 middle, middleUp;

        protected override void InitializeVars()
        {
            //velocity = transform.forward * (Time.deltaTime * bulletSpeed);
        }

        protected override void UpdateMoveDir()
        {
            sampleTime += Time.deltaTime * bulletSpeed;
            middle = (sourceTransform + target) / 2;
            middleUp = middle + (Vector3.up * 3);
            velocity = EvalBezier(sourceTransform, target, middleUp, sampleTime + 0.001f) - transform.position;

            if(sampleTime >= 1)
            {
                transform.gameObject.SetActive(false);
            }
        }

        protected override void OnCollide(RaycastHit hit)
        {
            if (hit.transform.CompareTag("Player"))
            {
                HurtAgent(hit.transform.GetComponent<Agent>());
            }
        }

        public Vector3 EvalBezier(Vector3 start, Vector3 end, Vector3 control, float t)
        {
            Vector3 ac = Vector3.Lerp(start, control, t);
            Vector3 cb = Vector3.Lerp(control, end, t);

            return Vector3.Lerp(ac, cb, t);
        }

        private void OnDrawGizmos()
        {
            if (middleUp != null) Gizmos.DrawSphere(middleUp, 0.1f);
            Gizmos.DrawSphere(target, 0.1f);
            Gizmos.DrawSphere(sourceTransform, 0.1f);
        }
    }
}

