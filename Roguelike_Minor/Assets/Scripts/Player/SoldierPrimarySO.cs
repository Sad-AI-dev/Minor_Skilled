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
            float inaccuracy = System.Convert.ToSingle(source.vars["inaccuracy"]);
            inaccuracy += source.coolDown * spreadBuildupSpeed;
            inaccuracy = Mathf.Clamp(inaccuracy, 0, 1);
            source.vars["inaccuracy"] = inaccuracy;
            //Debug.Log(source.vars["inaccuracy"]);
            Debug.Log("Increased: " + inaccuracy);

            //calculate spread amount
            float spread = spreadBuildup.Evaluate(inaccuracy) * spreadMultiplier;
            //Debug.Log(spread);

            //set bullet move direction
            UnityEngine.Vector3 offset = new UnityEngine.Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
            bulletDir = (target - source.originPoint.position).normalized;
            bulletDir += (offset * spread) / 25;
            bulletDir.Normalize();

            //spawn bullet and set its velocity
            GameObject projectile = Instantiate(bullet, source.originPoint.position, UnityEngine.Quaternion.identity);
            projectile.GetComponent<PlayerBullet>().moveDir = bulletDir * bulletSpeed;

            bool buildingDownSpread = Convert.ToBoolean(source.vars["buildingDownSpread"]);
            if (!buildingDownSpread)
            {
                source.agent.StartCoroutine(ReduceSpreadCo(source));
            }
        }

        private IEnumerator ReduceSpreadCo(Ability source)
        {
            bool buildingDownSpread = Convert.ToBoolean(source.vars["buildingDownSpread"]);
            buildingDownSpread = true;
            source.vars["buildingDownSpread"] = buildingDownSpread;
            float inaccuracy = System.Convert.ToSingle(source.vars["inaccuracy"]);
            while (inaccuracy > 0f)
            {
                yield return null;
                inaccuracy -= Time.deltaTime * spreadResetSpeed;
                source.vars["inaccuracy"] = inaccuracy;
                Debug.Log("reduced: " + inaccuracy);
                //Debug.Log("reduced");
            }
            //done cooling down
            source.vars["inaccuracy"] = 0f;
            buildingDownSpread = Convert.ToBoolean(source.vars["buildingDownSpread"]);
            buildingDownSpread = false ;
            source.vars["buildingDownSpread"] = buildingDownSpread;
        }
    }
}
