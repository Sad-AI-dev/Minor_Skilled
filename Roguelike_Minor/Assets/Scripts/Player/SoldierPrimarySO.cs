using Game.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game.Player
{
    [CreateAssetMenu(fileName = "SoldierPrimary", menuName = "ScriptableObjects/Agent/Ability/Soldier/SoldierPrimary")]
    public class SoldierPrimarySO : AbilitySO
    {
        public GameObject bullet;
        public GameObject marker;
        public float bulletSpeed;
        public float spreadMultiplier;
        public float spreadBuildupSpeed;
        public float spreadResetSpeed;
        public AnimationCurve spreadBuildup;

        
        public override void Use(Ability source)
        {
            Camera cam = Camera.main;
            Vector3 target;
            Vector3 bulletDir;

            RaycastHit hit;
            if (Physics.Raycast(cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)), out hit, 500))
            {
                target = hit.point;
                //Instantiate(marker, hit.point, Quaternion.identity);
            }
            else
            {
                target = cam.transform.forward * 1000;
            }

            GameObject projectile = Instantiate(bullet, source.originPoint.position, Quaternion.identity);

            float spread = Convert.ToSingle(source.vars["inaccuracy"]) * spreadBuildupSpeed;
            //float spreadReset = Convert.ToSingle(source.vars["accuracy"]) * spreadResetSpeed;
            if (spread >= 1) spread = 1;
            //spread -= spreadReset;
            if(spread < 0) spread = 0;
            float inaccuracy = spreadBuildup.Evaluate(spread) * spreadMultiplier;

            Debug.Log(inaccuracy);

            Vector3 offset = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));

            bulletDir = (target - source.originPoint.position) + (offset * inaccuracy);
            bulletDir.Normalize();
            projectile.GetComponent<PlayerBullet>().moveDir = bulletDir * bulletSpeed;
        }
    }
}
