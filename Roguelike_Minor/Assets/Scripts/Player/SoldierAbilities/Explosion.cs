using Game.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Game.Player.Soldier
{
    public class Explosion : MonoBehaviour
    {
        [SerializeField] private GameObject sphere;

        public Agent source;

        public float damage;
        public float areaRadius;

        public int ticks;
        public float tickDelay;

        private bool canTick = false;

        private List<Agent> agentsInRange;

        private void Start()
        {
            sphere.transform.localScale *= areaRadius;
        }

        private void LateUpdate()
        {
            if (canTick)
            {
                executeTick();
            }
        }

        private void executeTick()
        {
            canTick = false;

            HitEvent hitEvent = new HitEvent(source);
            hitEvent.baseDamage = damage;

            if(agentsInRange.Count > 0 )
            {
                foreach (Agent agent in agentsInRange)
                {
                    agent.health.Hurt(hitEvent);
                }
            }

            StartCoroutine(WaitForNextTickCo());
        }

        IEnumerator WaitForNextTickCo()
        {
            yield return new WaitForSeconds(tickDelay);
            ticks--;
            if (ticks >= 0) canTick = true;
            else Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Enemy"))
            {
                if(other.TryGetComponent(out Agent enemy))
                agentsInRange.Add(enemy);
            }
            if(other.TryGetComponent(out Agent player))
            {
                agentsInRange.Add(player);
            }
        }
    }
}
