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
        public float explosionDamage;

        [Header("Poison Grenade")]
        [SerializeField] private BehaviourPool<Projectile> grenades = new BehaviourPool<Projectile>();
        [SerializeField] private float poisonUpwardVelocity;
        [SerializeField] private float poisonGravity;
        [SerializeField] private float minDistance;
        [SerializeField] private float maxDistance;
        public int poisonGrenadeAmount;
        public float poisonGrenadeDamage;

        [HideInInspector] public float gravity;
        [HideInInspector] public float upwardVelocity;

        private float minAngle = 0;
        private float maxAngle;

        private bool addedVerticalVelocity = false;

        public void Start()
        {
            SetTrigger(false);
        }

        protected override void UpdateMoveDir()
        {
            if(!addedVerticalVelocity)
            {
                velocity += Vector3.up * upwardVelocity;
                addedVerticalVelocity = true;
            }

            velocity += new Vector3(0, -gravity * Time.deltaTime, 0);
        }

        protected override void CustomCollide(Collision collision)
        {
            List<Agent> agents = Explosion.FindAgentsInRange(transform.position, radius, source);
            Explosion.DealDamage(agents, source, explosionDamage);
            ScreenShakeManager.instance.ShakeCamera(5, 2, 1, transform.position);
            GameObject visualExplosion = Instantiate(visuals, transform.position, Quaternion.identity);
            visualExplosion.transform.localScale *= radius * 2;

            SFX.Post(gameObject);

            minAngle = 0;

            for (int i = 0; i < poisonGrenadeAmount; i++)
            {
                Vector3 colNormal = collision.GetContact(0).normal;
                Projectile projectile = grenades.GetBehaviour();
                projectile.transform.position = transform.position + new Vector3(0, 0.5f, 0);
                projectile.Initialize(ability);

                PoisonGrenade pGrenade = projectile.GetComponent<PoisonGrenade>();
                pGrenade.damage = poisonGrenadeDamage;
                pGrenade.velocity = Vector3.zero;

                maxAngle = (360 / poisonGrenadeAmount) * (i + 1);

                float angle = Random.Range(minAngle, maxAngle);

                Quaternion direction = Quaternion.AngleAxis(angle, colNormal);
                Vector3 directionVector = direction * Vector3.forward;
                directionVector.Normalize();

                float directionDistance = Random.Range(minDistance, maxDistance);

                Vector3 grenadeVelocity = colNormal * 2 + directionVector;
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
