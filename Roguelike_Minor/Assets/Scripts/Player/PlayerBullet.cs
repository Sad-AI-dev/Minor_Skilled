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

        public GameObject marker;

        protected virtual void FixedUpdate()
        {
            transform.position += moveDir;
        }

        private void Update()
        {
            CheckHitObject();
        }

        private void CheckHitObject()
        {
            RaycastHit hit;
            if(Physics.Raycast(transform.position, transform.forward, out hit, moveDir.magnitude))
            {
                if(hit.transform.CompareTag("Enemy"))
                {
                    if(hit.transform.TryGetComponent(out Agent enemy))
                    {
                        enemy.health.Hurt(new HitEvent(ability));
                    }
                }
                CustomOnCollide();

                if (!CompareTag("Player"))
                    Destroy(gameObject);
            }
        }

        protected virtual void CustomOnCollide()
        {

        }
    }
}
