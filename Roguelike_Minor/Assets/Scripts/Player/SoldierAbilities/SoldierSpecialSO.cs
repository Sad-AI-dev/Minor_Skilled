using Game.Core;
using Game.Core.GameSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player.Soldier
{
    [CreateAssetMenu(fileName = "SoldierSpecial", menuName = "ScriptableObjects/Agent/Ability/Soldier/Special")]
    public class SoldierSpecialSO : AbilitySO
    {
        [SerializeField] private LayerMask layermask;

        [Header("explosion")]
        [SerializeField] private GameObject explosion;
        [SerializeField] private float radius;
        [SerializeField] private float knockbackForce;

        PlayerController controller;

        public override void InitializeVars(Ability source)
        {
            controller = source.agent.GetComponent<PlayerController>();
        }

        public override void Use(Ability source)
        {
            Camera cam = Camera.main;
            Vector3 target;

            ScreenShakeManager.instance.ShakeCamera(3, 1, 1, source.agent.transform.position);
            


            controller.StartSlowCoroutine(.2f);

            RaycastHit hit;
            if (Physics.Raycast(cam.ViewportPointToRay(new UnityEngine.Vector3(0.5f, 0.5f, 0)), out hit, 500, layermask))
                target = hit.point;
            else
                return;

            if(hit.transform.TryGetComponent<Agent>(out Agent agent))
            {
                agent.health.Hurt(new HitEvent(source));
            }


            Explosion.DealKnockback(Explosion.FindAgentsInRange(target, radius), knockbackForce, target);
            GameObject projectile = Instantiate(explosion, target, Quaternion.identity);
            projectile.transform.localScale *= radius * 2;
        }
    }
}
