using Game.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player.Soldier
{
    [CreateAssetMenu(fileName = "SoldierSpecial", menuName = "ScriptableObjects/Agent/Ability/Soldier/Special")]
    public class SoldierSpecialSO : AbilitySO
    {
        [SerializeField] private GameObject bullet;
        [SerializeField] private float bulletSpeed;
        [SerializeField] private LayerMask layermask;

        PlayerController controller;

        public override void Use(Ability source)
        {
            Camera cam = Camera.main;
            Vector3 target;
            Vector3 bulletDir;

            if (!source.vars.ContainsKey("varsInitialized"))
                InitializeVars(source);

            controller.StartSlowCoroutine(.2f);

            RaycastHit hit;
            if (Physics.Raycast(cam.ViewportPointToRay(new UnityEngine.Vector3(0.5f, 0.5f, 0)), out hit, 500, layermask))
                target = hit.point;
            else
                target = cam.transform.forward * 1000;

            bulletDir = (target - source.originPoint.position).normalized;

            GameObject projectile = Instantiate(bullet, source.originPoint.position, Quaternion.identity);
            projectile.transform.LookAt(target);
            RailgunBullet rgBullet = projectile.GetComponent<RailgunBullet>();
            rgBullet.velocity = bulletDir * bulletSpeed;
            rgBullet.Initialize(source);
        }

        private void InitializeVars(Ability source)
        {
            source.vars.Add("varsInitialized", false);
            controller = source.agent.GetComponent<PlayerController>();
        }
    }
}
