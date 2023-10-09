using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game.Enemy
{
    public class PGSecondaryBehaviour : MonoBehaviour
    {
        public Ability source;
        public float bulletSpeed;
        float currentTime = 0;

        private void Update()
        {
            transform.position += transform.forward * bulletSpeed * Time.deltaTime;
            currentTime += Time.deltaTime;
            if(currentTime > 10)
            {
                gameObject.SetActive(false);
                currentTime = 0;
            }

            RaycastHit hit;
            if(Physics.Raycast(transform.position, transform.forward, out hit, bulletSpeed * Time.deltaTime))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    hit.transform.GetComponent<Agent>().health.Hurt(new HitEvent(source));
                }
                gameObject.SetActive(false);
            }
        }
    }
}
