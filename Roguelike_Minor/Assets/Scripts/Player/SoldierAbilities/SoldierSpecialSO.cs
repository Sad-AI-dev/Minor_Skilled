using Game.Core;
using Game.Core.GameSystems;
using JetBrains.Annotations;
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
        [SerializeField] private float explosionDamageMultiplier;
        private float explosionDamage;
 
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
            if (Physics.Raycast(cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)), out hit, 500, layermask))
                targetPos = hit.point;
            else
                return;

            if(hit.transform.TryGetComponent<Agent>(out Agent agent))
            {
                agent.health.Hurt(new HitEvent(source));
            }

            explosionDamage = source.agent.stats.baseDamage * explosionDamageMultiplier;

            List<Agent> targetAgents = Explosion.FindAgentsInRange(targetPos, radius);
            Explosion.DealDamage(targetAgents, source.agent, explosionDamage);
            Explosion.DealKnockback(targetAgents, knockbackForce, targetPos);
            GameObject projectile = Instantiate(explosion, targetPos, Quaternion.identity);
            projectile.transform.localScale *= radius * 0.8f;
        }
    }
}
