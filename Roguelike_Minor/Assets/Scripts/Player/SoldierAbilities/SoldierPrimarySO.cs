using Game.Core;
using Game.Core.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor;
using UnityEngine;

namespace Game.Player.Soldier
{
    [CreateAssetMenu(fileName = "SoldierPrimary", menuName = "ScriptableObjects/Agent/Ability/Soldier/SoldierPrimary")]
    public class SoldierPrimarySO : AbilitySO
    {
        public class PrimaryVars : Ability.AbilityVars
        {
            public float inaccuracy = 0;
            public bool buildingDownSpread = false;
            public bool isShooting = false;
            public Coroutine stopShootingCo;
            public BehaviourPool<Projectile> bulletPool = new BehaviourPool<Projectile>();
            public LineRenderer lineRenderer;
            public float lineOpacity;
        }

        public GameObject bullet;
        public GameObject marker;
        public float bulletSpeed;
        public float spreadMultiplier;
        public float spreadBuildupSpeed;
        public float spreadResetSpeed;
        public AnimationCurve spreadBuildup;
        public LayerMask layermask;

        PlayerController controller;

        public override void InitializeVars(Ability source)
        {
            source.vars = new PrimaryVars
            {
                inaccuracy = 0,
                buildingDownSpread = false,
                isShooting = false,
                stopShootingCo = null,
                bulletPool = new BehaviourPool<Projectile>(),
                lineRenderer = source.agent.GetComponent<LineRenderer>(),
                lineOpacity = 255
            };

            PrimaryVars vars = source.vars as PrimaryVars;
            vars.bulletPool.behaviourTemplate = bullet;
            controller = source.agent.GetComponent<PlayerController>();
        }

        public override void Use(Ability source)
        {
            Camera cam = Camera.main;
            UnityEngine.Vector3 target = UnityEngine.Vector3.zero;
            UnityEngine.Vector3 bulletDir;
            

            PrimaryVars vars = source.vars as PrimaryVars;

            ScreenShakeManager.instance.ShakeCamera(1, 1, source.agent.transform.position);

            controller.StartSlowCoroutine(source.coolDown * 1.1f);

            RaycastHit hit;
            RaycastHit _hit = new RaycastHit();
            if (Physics.Raycast(cam.ViewportPointToRay(new UnityEngine.Vector3(0.5f, 0.5f, 0)), out hit, 500, layermask))
            {
                if (Physics.Raycast(source.originPoint.position, hit.point - source.originPoint.position, out _hit, 500, layermask))
                    target = _hit.point;
            }
            else
                return;

            if (_hit.transform.TryGetComponent<Agent>(out Agent agent))
            {
                agent.health.Hurt(new HitEvent(source));
            }

            vars.lineRenderer.enabled = true;
            UnityEngine.Vector3[] positions = { source.originPoint.position, target};
            vars.lineRenderer.SetPositions(positions);
            vars.lineRenderer.widthMultiplier = 0.2f;
            vars.lineOpacity = 1;

            source.agent.StartCoroutine(ReduceLineOpacity(vars, source));

            /*//add inaccuracy
            vars.inaccuracy += source.coolDown * spreadBuildupSpeed;
            vars.inaccuracy = Mathf.Clamp(vars.inaccuracy, 0, 1);
            //Debug.Log("Increased: " + inaccuracy);

            //calculate spread amount
            float spread = spreadBuildup.Evaluate(vars.inaccuracy) * spreadMultiplier;

            //set bullet move direction
            UnityEngine.Vector3 offset = new UnityEngine.Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
            bulletDir = (target - source.originPoint.position).normalized;
            bulletDir += (offset * spread) / 25;
            bulletDir.Normalize();

            //set isShooting boolean
            vars.isShooting = true;


            //spawn bullet and set its velocit

            Projectile projectile = vars.bulletPool.GetBehaviour();
            //Instantiate(bullet, source.originPoint.position, UnityEngine.Quaternion.identity);
            projectile.transform.position = source.originPoint.position;
            projectile.transform.LookAt(target);

            //initialize projectile
            RifleBullet bullet = projectile.GetComponent<RifleBullet>();
            bullet.velocity = bulletDir * bulletSpeed;
            bullet.Initialize(source);

            Coroutine shootRoutine = vars.stopShootingCo;
            if(shootRoutine != null)
            {
                source.agent.StopCoroutine(shootRoutine);
                vars.buildingDownSpread = false;
            }

            if (!vars.buildingDownSpread)
            {
                shootRoutine = source.agent.StartCoroutine(IsShootingCo(source));
                vars.stopShootingCo = shootRoutine;
                //Debug.Log("Started counting down");
            }*/
        }

        private IEnumerator IsShootingCo(Ability source)
        {
            PrimaryVars vars = source.vars as PrimaryVars;
            vars.buildingDownSpread = true;
            float timeElapsed = 0;
            while (timeElapsed < source.coolDown * 2)
            {
                yield return null;
                timeElapsed += Time.deltaTime;
            }
            //stopped shooting
            //Debug.Log("Stopped shooting");
            vars.isShooting = false;
            vars.buildingDownSpread = false;

            source.agent.StartCoroutine(ReduceSpreadCo(source));
        }

        private IEnumerator ReduceSpreadCo(Ability source)
        {
            PrimaryVars vars = source.vars as PrimaryVars;

            while (vars.inaccuracy > 0f && !vars.isShooting)
            {
                yield return null;
                vars.inaccuracy -= Time.deltaTime * spreadResetSpeed;
                //Debug.Log("reduced: " + inaccuracy);
            }
            //done cooling down
            if (!vars.isShooting)
                vars.inaccuracy = 0;
        }

        private IEnumerator ReduceLineOpacity(PrimaryVars vars, Ability source)
        {
            /*            while(vars.lineOpacity > 0)
                        {
                            yield return null;
                            Debug.Log("cancer");
                            Color color = new Color(158, 52, 235, vars.lineOpacity);
                            vars.lineRenderer.startColor = color;
                            vars.lineRenderer.endColor = color;
                            vars.lineOpacity -= 0.1f;
                        }*/

            yield return new WaitForSeconds(source.coolDown/2);
            vars.lineRenderer.enabled = false;
        }
    }
}
