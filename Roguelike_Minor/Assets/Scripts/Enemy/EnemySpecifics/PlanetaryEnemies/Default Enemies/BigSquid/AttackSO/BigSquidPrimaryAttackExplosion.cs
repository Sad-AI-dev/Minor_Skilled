using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Core.GameSystems;

namespace Game.Enemy
{
    public class BigSquidPrimaryAttackExplosion : MonoBehaviour
    {
        public float explosionRange = 3;
        public float knockbackForce = 1;

        private void Start()
        {
            StartCoroutine(HandleExplosion());
        }

        IEnumerator HandleExplosion()
        {
            List<Agent> agents = Explosion.FindAgentsInRange(transform.position, explosionRange);
            if (agents.Count > 0)
            {
                Explosion.DealKnockback(agents, knockbackForce, transform.position);
            }
            yield return new WaitForSeconds(0.2f);
            Destroy(gameObject);

        }
    }
}
