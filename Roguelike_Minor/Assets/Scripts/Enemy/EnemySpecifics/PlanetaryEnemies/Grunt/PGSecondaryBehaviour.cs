using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game.Enemy
{
    public class PGSecondaryBehaviour : Projectile
    {
        [HideInInspector] public Transform target;
        [HideInInspector] public float bulletSpeed;

        protected override void InitializeVars()
        {
            transform.LookAt(target.position + Vector3.up);
            velocity = transform.forward * (bulletSpeed * Time.deltaTime);
        }

        protected override void OnCollide(RaycastHit hit)
        {
            if (hit.transform.CompareTag("Player"))
            {
                HurtAgent(hit.transform.GetComponent<Agent>());
            }
        }
    }
}
