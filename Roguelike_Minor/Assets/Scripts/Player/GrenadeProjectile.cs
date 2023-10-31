using Game.Core;
using Game.Core.Data;
using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] private float spreadMultiplier;
        [SerializeField] private AK.Wwise.Event SFX;

        private float minAngle = 0;
        private float maxAngle;

        protected override void UpdateMoveDir()
        {
            velocity += new Vector3(0, -gravity * Time.deltaTime, 0);
        }

        protected override void OnCollide(RaycastHit hit)
        {
            Instantiate(explosion, hit.point, Quaternion.identity);

            SFX.Post(gameObject);

            for(int i = 0; i < poisonGrenadeAmount; i++)
            {
                Projectile projectile = grenades.GetBehaviour();
                projectile.transform.position = hit.point;
                projectile.Initialize(ability);

                PoisonGrenade pGrenade = projectile.GetComponent<PoisonGrenade>();

                Vector3 directionOffset = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));



                Quaternion direction = Quaternion.AngleAxis((360 / poisonGrenadeAmount) * i + 1, Vector3.up);
                Vector3 angleVector = direction * Vector3.forward;

                Vector3 grenadeVelocity = projectile.transform.up + (directionOffset * spreadMultiplier);
                grenadeVelocity.Normalize();

                pGrenade.velocity = grenadeVelocity * poisonGrenadeSpeed;
            }
        }
    }
}
