using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core.GameSystems;
using Game.Core;


namespace Game.Enemy
{
    public class SpikeRise : MonoBehaviour
    {
        Vector3 target;
        public float speed = 4f;
        bool stopped = false;
        bool canHit = false;

        private void Start()
        {
            target = transform.position;
            target.y = target.y + 9;
        }

        private void Update()
        {
            if(transform.position.y < 2 && !stopped)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            }
            else
            {
                stopped = true;
            }
        }

        public IEnumerator launched()
        {
            yield return new WaitForSeconds(2);
            canHit = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (canHit)
            {
                if(other != null)
                {
                    //TODO: Explode;
                    float distance = Vector3.Distance(transform.position, GameStateManager.instance.player.transform.position);
                    if (distance <= 10)
                    {
                        Explosion.DealDamage(new List<Agent>() { GameStateManager.instance.player }, transform.GetComponent<Agent>(), 10);
                    }

                    Destroy(this.gameObject);
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, 10);
        }
    }
}
