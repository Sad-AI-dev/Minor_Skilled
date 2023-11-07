using Game.Core;
using Game.Core.Data;
using UnityEngine;

namespace Game.Player
{
    public class GrenadeProjectile : Projectile
    {
        [SerializeField] private float gravity;
        [SerializeField] private GameObject explosion;

        [Header("Poison Grenade")]
        [SerializeField] private BehaviourPool<Projectile> grenades = new BehaviourPool<Projectile>();
        [SerializeField] private float poisonGrenadeSpeed;
        [SerializeField] private int poisonGrenadeAmount;
        [SerializeField] private float minDistance;
        [SerializeField] private float maxDistance;
        [SerializeField] private AK.Wwise.Event SFX;

        private float minAngle = 0;
        private float maxAngle;

        private bool addedVerticalVelocity = false;

        protected override void UpdateMoveDir()
        {
            if(!addedVerticalVelocity)
            {
                velocity += Vector3.up * 0.2f;
                addedVerticalVelocity = true;
            }

            velocity += new Vector3(0, -gravity * Time.deltaTime, 0);
        }

        protected override void OnCollide(RaycastHit hit)
        {
            Instantiate(explosion, hit.point, Quaternion.identity);

            SFX.Post(gameObject);

            minAngle = 0;

            /*for (int i = 0; i < poisonGrenadeAmount; i++)
            {
                Projectile projectile = grenades.GetBehaviour();
                projectile.transform.position = hit.point;
                projectile.Initialize(ability);

                PoisonGrenade pGrenade = projectile.GetComponent<PoisonGrenade>();

                Vector3 directionOffset = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));

                maxAngle = (360 / poisonGrenadeAmount) * (i + 1);

                float angle = Random.Range(minAngle, maxAngle);

                Quaternion direction = Quaternion.AngleAxis(angle, Vector3.up);
                Vector3 directionVector = direction * Vector3.forward;
                directionVector.Normalize();

                float directionDistance = Random.Range(minDistance, maxDistance);

                Vector3 grenadeVelocity = projectile.transform.up + directionVector;
                grenadeVelocity.Normalize();
                grenadeVelocity *= directionDistance;

                pGrenade.velocity = grenadeVelocity;
                minAngle = maxAngle;
                maxAngle = 0;
            }*/
        }
    }
}
