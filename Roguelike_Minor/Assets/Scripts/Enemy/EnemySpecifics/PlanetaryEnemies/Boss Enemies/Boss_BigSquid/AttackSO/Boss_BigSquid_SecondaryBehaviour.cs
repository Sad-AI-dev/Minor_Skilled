using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Game.Enemy.Core;
using Game.Core;

namespace Game.Enemy {
    public class Boss_BigSquid_SecondaryBehaviour : MonoBehaviour
    {
        public Rigidbody rb;
        public float bulletSpeed;
        public Ability source;

        private void Update()
        {
            if(transform.parent != null)
            {
                transform.position = transform.parent.position;
            }
        }

        public void Fire(Transform target)
        {
            Vector3 dir = (target.position + (Random.insideUnitSphere * 4f) - transform.position).normalized;
            bulletSpeed *= 100;
            rb.AddForce(dir * bulletSpeed, ForceMode.Force);
        }

        public IEnumerator RandomFireCo(Transform target)
        {
            yield return new WaitForSeconds(Random.Range(0, 0.2f));
            Fire(target);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (transform.parent == null)
            {
                if (other.transform.CompareTag("Player"))
                {
                    other.GetComponent<Agent>().health.Hurt(new HitEvent(source));
                }

                Destroy(this.gameObject);
            }
        }
    }
}
