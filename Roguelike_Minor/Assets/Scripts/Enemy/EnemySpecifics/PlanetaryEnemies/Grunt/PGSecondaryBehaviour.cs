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
        [HideInInspector] public Ability abil;

        private void Start()
        {
            transform.LookAt(target.position + Vector3.up);
            velocity = transform.forward * bulletSpeed * Time.deltaTime;
            ability = this.abil;
            this.source = abil.agent;
        }

        protected override void OnCollide(RaycastHit hit)
        {
            if (hit.transform.CompareTag("Player"))
            {
                hit.transform.GetComponent<Agent>().health.Hurt(new HitEvent(source));
            }
        }
    }
}
