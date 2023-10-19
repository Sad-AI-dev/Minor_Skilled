using Game.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public class PoisonGrenade : Projectile
    {
        [SerializeField] private float gravity;
        [SerializeField] private GameObject poisonCloud;


        protected override void UpdateMoveDir()
        {
            velocity += new Vector3(0, -gravity * Time.deltaTime, 0);
        }

        protected override void OnCollide(RaycastHit hit)
        {
            Instantiate(poisonCloud, hit.point, Quaternion.identity);
        }
    }
}
