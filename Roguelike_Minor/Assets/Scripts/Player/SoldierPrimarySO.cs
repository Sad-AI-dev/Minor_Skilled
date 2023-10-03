using Game.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game.Player
{
    [CreateAssetMenu(fileName = "SoldierPrimary", menuName = "ScriptableObjects/Agent/Ability/Soldier/SoldierPrimary")]
    public class SoldierPrimarySO : AbilitySO
    {
        [SerializeField] private GameObject bullet;
        [SerializeField] private GameObject marker;
        [SerializeField] private float bulletSpeed;
        
        public override void Use(Ability source)
        {
            Camera cam = Camera.main;
            Vector3 target;
            Vector3 bulletDir;

            RaycastHit hit;
            if(Physics.Raycast(cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)), out hit, 500))
            {
                target = hit.point;
                //Instantiate(marker, hit.point, Quaternion.identity);
            }
            else
            {
                target = cam.transform.forward * 10000;
            }

            GameObject projectile = Instantiate(bullet, source.originPoint.position, Quaternion.identity);

            bulletDir = (target - source.originPoint.position).normalized;
            projectile.GetComponent<PlayerBullet>().moveDir = bulletDir * bulletSpeed;
        }
    }
}
