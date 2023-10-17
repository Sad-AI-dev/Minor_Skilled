using Game.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public class RailgunBullet : Projectile
    {
        [SerializeField] private GameObject explosion;

        protected override void OnCollide(RaycastHit hit)
        {
            if (hit.transform.TryGetComponent(out Agent enemy))
            {
                enemy.health.Hurt(new HitEvent(ability));
            }

            Instantiate(explosion, hit.point, Quaternion.identity);
        }
    }
}
