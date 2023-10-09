using Game.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public class PlayerBullet : MonoBehaviour
    {
        [HideInInspector] public Vector3 moveDir;
        public Ability ability;
        public Agent source;

        protected virtual void FixedUpdate()
        {
            transform.position += moveDir;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject.CompareTag("Enemy"))
            {
                if(collision.gameObject.TryGetComponent(out Agent enemy))
                {
                    enemy.health.Hurt(new HitEvent(ability));
                }
            }

            CustomOnCollide();

            if(!CompareTag("Player"))
                Destroy(gameObject);
        }

        protected virtual void CustomOnCollide()
        {

        }
    }
}
