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
        [SerializeField] private float projectileVelocity;
        [SerializeField] private float upwardVelocity;
        [SerializeField] private float projectileGravity;


        PlayerController controller;

        public override void InitializeVars(Ability source)
        {
            controller = source.agent.GetComponent<PlayerController>();
        }

        public override void Use(Ability source)
        {
            Camera cam = Camera.main;
            Vector3 target;
            Vector3 bulletDir;

            controller.StartSlowCoroutine(.2f);

            RaycastHit hit;
            if (Physics.Raycast(cam.ViewportPointToRay(new UnityEngine.Vector3(0.5f, 0.5f, 0)), out hit, 500, layermask))
                target = hit.point;
            else
                target = cam.transform.forward * 1000;

            bulletDir = (target - source.originPoint.position).normalized;

            GameObject projectile = Instantiate(grenade, source.originPoint.position, Quaternion.identity);
            projectile.transform.LookAt(target);
            GrenadeProjectile grenadeProjectile = projectile.GetComponent<GrenadeProjectile>();
            grenadeProjectile.velocity = bulletDir * projectileVelocity;
            grenadeProjectile.Initialize(source);
            grenadeProjectile.gravity = projectileGravity;
            grenadeProjectile.upwardVelocity = upwardVelocity;
        }
    }
}
