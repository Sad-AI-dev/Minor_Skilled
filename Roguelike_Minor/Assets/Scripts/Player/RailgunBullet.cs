using Game.Core;
using Game.Core.GameSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace Game.Player
{
    public class RailgunBullet : Projectile
    {
        [SerializeField] private GameObject explosion;
        [SerializeField] private float explosionRadius;

        Explosion _explosion = new Explosion();

        protected override void OnCollide(RaycastHit hit)
        {
            if (hit.transform.TryGetComponent(out Agent enemy))
            {
                HurtAgent(enemy);
            }

            List<Agent> agents = _explosion.FindAgentsInRange(hit.point, explosionRadius, true);
            _explosion.DealDamage(source, 10, agents);
            _explosion.DealKnockback(2f, hit.point, agents);
            GameObject visuals = Instantiate(explosion, hit.point, Quaternion.identity);
            visuals.transform.localScale *= explosionRadius * 2;
        }
    }
}
