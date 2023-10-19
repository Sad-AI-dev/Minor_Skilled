using Game.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Game.Player.Soldier
{
    [CreateAssetMenu(fileName = "SoldierSecondary", menuName = "ScriptableObjects/Agent/Ability/Soldier/Secondary")]
    public class SoldierSecondary : AbilitySO
    {
        [SerializeField] private GameObject grenade;
        [SerializeField] private LayerMask layermask;
        [SerializeField] private float bulletSpeed;

        public override void Use(Ability source)
        {
            Camera cam = Camera.main;
            Vector3 target;
            Vector3 bulletDir;

            RaycastHit hit;
            if (Physics.Raycast(cam.ViewportPointToRay(new UnityEngine.Vector3(0.5f, 0.5f, 0)), out hit, 500, layermask))
                target = hit.point;
            else
                target = cam.transform.forward * 1000;

            bulletDir = (target - source.originPoint.position).normalized;

            GameObject projectile = Instantiate(grenade, source.originPoint.position, Quaternion.identity);
            projectile.transform.LookAt(target);
            GrenadeProjectile grenadeProjectile = projectile.GetComponent<GrenadeProjectile>();
            grenadeProjectile.velocity = bulletDir * bulletSpeed;
            grenadeProjectile.Initialize(source);
        }
    }
}
