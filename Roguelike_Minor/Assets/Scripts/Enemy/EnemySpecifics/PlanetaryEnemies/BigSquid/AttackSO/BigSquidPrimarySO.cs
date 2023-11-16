using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Enemy.Core;
using Game.Core;


namespace Game.Enemy {
    public class BigSquidPrimaryVars : Ability.AbilityVars
    {
        public Transform target;
        public LineRenderer lineRenderer;
    }

    [CreateAssetMenu(fileName = "BigSquidPrimary", menuName = "ScriptableObjects/Enemy/BigSquid/PrimaryAttack")]

    public class BigSquidPrimarySO : AbilitySO
    {
        public GameObject explosion;
        
        public override void InitializeVars(Ability source)
        {

        }

        public override void Use(Ability source)
        {
            source.agent.StartCoroutine(TargetingCo(source));   
        }

        IEnumerator TargetingCo(Ability source)
        {
            BigSquidPrimaryVars vars = source.vars as BigSquidPrimaryVars;

            //Targeting
            vars.lineRenderer.enabled = true;

            yield return new WaitForSeconds(3);

            //Shooting
            vars.lineRenderer.enabled = false;
            Vector3 shootPos = vars.target.position + Vector3.up;

            yield return new WaitForSeconds(0.2f);
            FireLineRenderer(vars, source.originPoint.position, shootPos);
            attack(source, shootPos);

            yield return new WaitForSeconds(0.1f);

            vars.lineRenderer.enabled = false;
        }

        void attack(Ability source, Vector3 target)
        {
            Vector3 dir = (target - source.originPoint.position).normalized;

            RaycastHit hit;
            if (Physics.Raycast(source.originPoint.position, dir, out hit, Mathf.Infinity))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    hit.transform.GetComponent<Agent>().health.Hurt(new HitEvent(source));
                }

                Instantiate(explosion, hit.point, Quaternion.identity);
            }
        }



        void FireLineRenderer(BigSquidPrimaryVars vars, Vector3 start, Vector3 end)
        {
            vars.lineRenderer.enabled = true;
            vars.lineRenderer.SetPosition(0, start);
            vars.lineRenderer.SetPosition(1, end);
        }
    }
}
