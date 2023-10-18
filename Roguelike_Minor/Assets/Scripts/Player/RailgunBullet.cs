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
                HurtAgent(enemy);
            }

            Instantiate(explosion, hit.point, Quaternion.identity);
        }
    }
}
