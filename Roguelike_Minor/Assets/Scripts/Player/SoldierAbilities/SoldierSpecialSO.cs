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
        [SerializeField] private int damage;
 
        private PlayerController controller;
        private Camera cam;

        public override void InitializeVars(Ability source)
        {
            controller = source.agent.GetComponent<PlayerController>();
            cam = controller.cam;
        }

        public override void Use(Ability source)
        {
            Vector3 targetPos;

            ScreenShakeManager.instance.ShakeCamera(3, 1, 1, source.agent.transform.position);

            controller.StartSlowCoroutine(.2f);

            RaycastHit hit;
            if (Physics.Raycast(cam.ViewportPointToRay(new UnityEngine.Vector3(0.5f, 0.5f, 0)), out hit, 500, layermask))
                targetPos = hit.point;
            else
                return;

            if(hit.transform.TryGetComponent<Agent>(out Agent agent))
            {
                agent.health.Hurt(new HitEvent(source));
            }

            List<Agent> targetAgents = Explosion.FindAgentsInRange(targetPos, radius);
            Explosion.DealDamage(targetAgents, source.agent, damage);
            Explosion.DealKnockback(targetAgents, knockbackForce, targetPos);
            GameObject projectile = Instantiate(explosion, targetPos, Quaternion.identity);
            projectile.transform.localScale *= radius * 2;
        }
    }
}
