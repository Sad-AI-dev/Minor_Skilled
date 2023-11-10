using Game.Core;
using Game.Core.Data;
using Game.Core.GameSystems;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Player
{
    public class GrenadeProjectile : Projectile
    {
        [Header("General")]
        [SerializeField] private GameObject visuals;
        [SerializeField] private AK.Wwise.Event SFX;

        [Header("Explosion")]
        [SerializeField] private float radius;
        [SerializeField] private int explosionDamage;

        [Header("Poison Grenade")]
        [SerializeField] private BehaviourPool<Projectile> grenades = new BehaviourPool<Projectile>();
        [SerializeField] private float poisonUpwardVelocity;
        [SerializeField] private float poisonGravity;
        [SerializeField] private int poisonGrenadeAmount;
        [SerializeField] private float minDistance;
        [SerializeField] private float maxDistance;

        [HideInInspector] public float gravity;
        [HideInInspector] public float upwardVelocity;

        private float minAngle = 0;
        private float maxAngle;

        private bool addedVerticalVelocity = false;

        protected override void UpdateMoveDir()
        {
            if(!addedVerticalVelocity)
            {
                velocity += Vector3.up * upwardVelocity;
                addedVerticalVelocity = true;
            }

            velocity += new Vector3(0, -gravity * Time.deltaTime, 0);
        }

        protected override void CustomCollide(Collider other)
        {
            List<Agent> agents = Explosion.FindAgentsInRange(transform.position, radius, source);
            Explosion.DealDamage(agents, source, explosionDamage);
            GameObject visualExplosion = Instantiate(visuals, transform.position, Quaternion.identity);
            visualExplosion.transform.localScale *= radius * 2;

            SFX.Post(gameObject);

            minAngle = 0;

            for (int i = 0; i < poisonGrenadeAmount; i++)
            {
                Projectile projectile = grenades.GetBehaviour();
                projectile.transform.position = transform.position + new Vector3(0, 0.5f, 0);
                projectile.Initialize(ability);

                PoisonGrenade pGrenade = projectile.GetComponent<PoisonGrenade>();

                maxAngle = (360 / poisonGrenadeAmount) * (i + 1);

                float angle = Random.Range(minAngle, maxAngle);

                Quaternion direction = Quaternion.AngleAxis(angle, Vector3.up);
                Vector3 directionVector = direction * Vector3.forward;
                directionVector.Normalize();

                float directionDistance = Random.Range(minDistance, maxDistance);

                Vector3 grenadeVelocity = projectile.transform.up + directionVector;
                grenadeVelocity.Normalize();
                grenadeVelocity = new Vector3(grenadeVelocity.x * directionDistance, grenadeVelocity.y * poisonUpwardVelocity, grenadeVelocity.z *  directionDistance);

                pGrenade.gravity = poisonGravity;
                pGrenade.velocity = grenadeVelocity;
                minAngle = maxAngle;
                maxAngle = 0;
            }
        }
    }
}
