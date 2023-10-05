using Game.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
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
            UnityEngine.Vector3 target;
            UnityEngine.Vector3 bulletDir;

            if(!source.vars.ContainsKey("inaccuracy"))
                source.vars.Add("inaccuracy", 0);
            if (!source.vars.ContainsKey("buildingDownSpread"))
                source.vars.Add("buildingDownSpread", false);
            if (!source.vars.ContainsKey("isShooting"))
                source.vars.Add("isShooting", false);
            if (!source.vars.ContainsKey("stopShootingCo"))
                source.vars.Add("stopShootingCo", null);

            RaycastHit hit;
            if (Physics.Raycast(cam.ViewportPointToRay(new UnityEngine.Vector3(0.5f, 0.5f, 0)), out hit, 500))
            {
                target = hit.point;
            }
            else
            {
                target = cam.transform.forward * 1000;
            }
           
            //add inaccuracy
            float inaccuracy = Convert.ToSingle(source.vars["inaccuracy"]);
            inaccuracy += source.coolDown * spreadBuildupSpeed;
            inaccuracy = Mathf.Clamp(inaccuracy, 0, 1);
            source.vars["inaccuracy"] = inaccuracy;
            Debug.Log("Increased: " + inaccuracy);

            //calculate spread amount
            float spread = spreadBuildup.Evaluate(inaccuracy) * spreadMultiplier;

            //set bullet move direction
            UnityEngine.Vector3 offset = new UnityEngine.Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
            bulletDir = (target - source.originPoint.position).normalized;
            bulletDir += (offset * spread) / 25;
            bulletDir.Normalize();

            //set isShooting boolean
            bool isShooting = Convert.ToBoolean(source.vars["isShooting"]);
            isShooting = true;
            source.vars["isShooting"] = isShooting;


            //spawn bullet and set its velocity
            GameObject projectile = Instantiate(bullet, source.originPoint.position, UnityEngine.Quaternion.identity);
            projectile.GetComponent<PlayerBullet>().moveDir = bulletDir * bulletSpeed;

            bool buildingDownSpread = Convert.ToBoolean(source.vars["buildingDownSpread"]);

            Coroutine shootRoutine = (Coroutine)source.vars["stopShootingCo"];
            if(shootRoutine != null)
            {
                source.agent.StopCoroutine(shootRoutine);
                buildingDownSpread = false;
            }
                

            if (!buildingDownSpread)
            {
                shootRoutine = source.agent.StartCoroutine(IsShootingCo(source));
                source.vars["stopShootingCo"] = shootRoutine;
                Debug.Log("Started counting down");
            }
        }

        private IEnumerator IsShootingCo(Ability source)
        {
            bool buildingDownSpread = Convert.ToBoolean(source.vars["buildingDownSpread"]);
            buildingDownSpread = true;
            source.vars["buildingDownSpread"] = buildingDownSpread;
            float timeElapsed = 0;
            while (timeElapsed < source.coolDown * 2)
            {
                yield return null;
                timeElapsed += Time.deltaTime;
            }
            //stopped shooting
            Debug.Log("Stopped shooting");
            bool isShooting = Convert.ToBoolean(source.vars["isShooting"]);
            isShooting = false;
            source.vars["isShooting"] = isShooting;
            buildingDownSpread = false;
            source.vars["buildingDownSpread"] = buildingDownSpread;

            source.agent.StartCoroutine(ReduceSpreadCo(source));
        }

        private IEnumerator ReduceSpreadCo(Ability source)
        {
            bool isShooting = Convert.ToBoolean(source.vars["isShooting"]);
            float inaccuracy = Convert.ToSingle(source.vars["inaccuracy"]);
            while (inaccuracy > 0f && !isShooting)
            {
                yield return null;
                isShooting = Convert.ToBoolean(source.vars["isShooting"]);
                inaccuracy = Convert.ToSingle(source.vars["inaccuracy"]);
                inaccuracy -= Time.deltaTime * spreadResetSpeed;
                source.vars["inaccuracy"] = inaccuracy;
                //Debug.Log("reduced: " + inaccuracy);
            }
            //done cooling down
            if(!isShooting)
                source.vars["inaccuracy"] = 0f;
        }
    }
}
