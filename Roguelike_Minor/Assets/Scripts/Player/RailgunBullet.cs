using Game.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public class RailgunBullet : Projectile
    {
        protected override void OnCollide(RaycastHit hit)
        {
            if (hit.transform.TryGetComponent(out Agent enemy))
            {
                enemy.health.Hurt(new HitEvent(ability));
            }
        }
    }
}
