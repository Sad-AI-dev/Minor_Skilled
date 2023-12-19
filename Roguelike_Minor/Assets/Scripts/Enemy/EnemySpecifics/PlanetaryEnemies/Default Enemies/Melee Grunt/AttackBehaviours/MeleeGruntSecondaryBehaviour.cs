using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game.Enemy {
    public class MeleeGruntSecondaryBehaviour : Projectile
    {
        [HideInInspector] public Vector3 target;
        Vector3 acceleration = Vector3.down;
        [HideInInspector] public Vector3 sourceTransform;
        [HideInInspector] public float bulletSpeed;
        [HideInInspector] public float sampleTime;
        Vector3 middle, middleUp;

        protected override void InitializeVars()
        {
            transform.LookAt(target + Vector3.up);
            //velocity = transform.forward * (Time.deltaTime * bulletSpeed);

            //TODO: Calculate the amount of time for a bullet to reach the target 
            //velocity = GetLaunchVelocity(0.5f, transform.position, target);
        }

        private Vector3 GetLaunchVelocity(float flightTime, Vector3 startingPoint, Vector3 endPoint)
        {
            Vector3 gravityNormal = Physics.gravity.normalized;
            Vector3 dx = Vector3.ProjectOnPlane(endPoint, gravityNormal) - Vector3.ProjectOnPlane(startingPoint, gravityNormal);
            Vector3 initialVelocityX = dx / flightTime;

            Vector3 dy = Vector3.Project(endPoint, gravityNormal) - Vector3.Project(startingPoint, gravityNormal);
            Vector3 g = 0.5f * Physics.gravity * (flightTime * flightTime);
            Vector3 initialVelocityY = (dy - g) / flightTime;
            return initialVelocityX + initialVelocityY;
        }
        protected override void UpdateMoveDir()
        {
            //AgnleWithBezier();
            //CalculateVelocity();

            velocity = transform.forward * (Time.deltaTime * (bulletSpeed * 100));
        }
        private void AgnleWithBezier()
        {
            sampleTime += Time.deltaTime * bulletSpeed;
            middle = (sourceTransform + target) / 2;
            middleUp = middle + (Vector3.up * 3);
            velocity = EvalBezier(sourceTransform, target, middleUp, sampleTime + 0.001f) - transform.position;

            if (sampleTime >= 1)
            {
                transform.gameObject.SetActive(false);
            }
        }

        protected override void CustomCollide(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                HurtAgent(other.transform.GetComponent<Agent>());
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

