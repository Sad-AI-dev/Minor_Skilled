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
        [SerializeField] private float knockbackForce;

        protected override void OnCollide(RaycastHit hit)
        {
            if (hit.transform.TryGetComponent(out Agent enemy))
            {
                HurtAgent(enemy);
            }

            List<Agent> agents = Explosion.FindAgentsInRange(hit.point, explosionRadius);
            Explosion.DealDamage(agents, source, 10);
            Explosion.DealKnockback(agents, knockbackForce, hit.point);
            GameObject visuals = Instantiate(explosion, hit.point, Quaternion.identity);
            visuals.transform.localScale *= explosionRadius * 2;
        }
    }
}
