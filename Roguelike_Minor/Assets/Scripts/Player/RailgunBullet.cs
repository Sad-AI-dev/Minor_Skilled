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

        protected override void CustomCollide(Collider other)
        {
            if (other.TryGetComponent(out Agent enemy))
            {
                HurtAgent(enemy);
            }

            List<Agent> agents = Explosion.FindAgentsInRange(transform.position, explosionRadius);
            Explosion.DealDamage(agents, source, 10);
            Explosion.DealKnockback(agents, knockbackForce, transform.position);
            GameObject visuals = Instantiate(explosion, transform.position, Quaternion.identity);
            visuals.transform.localScale *= explosionRadius * 2;
        }
    }
}
