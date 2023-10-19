using Game.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public class GrenadeProjectile : Projectile
    {
        [SerializeField] private float gravity;

        protected override void UpdateMoveDir()
        {
            velocity += new Vector3(0, -gravity, 0);
        }
    }
}
