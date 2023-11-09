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
        

        Explosion _explosion = new Explosion();

        protected override void OnCollide(RaycastHit hit)
        {
            if (hit.transform.TryGetComponent(out Agent enemy))
            {
                HurtAgent(enemy);
            }

            List<Agent> agents = _explosion.FindAgentsInRange(hit.point, explosionRadius);
            _explosion.DealDamage(agents, source, 10);
            _explosion.DealKnockback(agents, knockbackForce, hit.point);
            GameObject visuals = Instantiate(explosion, hit.point, Quaternion.identity);
            visuals.transform.localScale *= explosionRadius * 2;
        }
    }
}
