using Game.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public class RifleBullet : Projectile
    {
        protected override void OnCollide(RaycastHit hit)
        {
            if (hit.transform.TryGetComponent(out Agent enemy))
            {
                HurtAgent(enemy);
            }
        }

        protected override void UpdateMoveDir()
        {
            base.UpdateMoveDir();
        }
    }
}
