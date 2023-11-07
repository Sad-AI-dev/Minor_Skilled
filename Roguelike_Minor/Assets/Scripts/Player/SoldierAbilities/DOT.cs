using Game.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Game.Player.Soldier
{
    public class DOT : MonoBehaviour
    {
        [SerializeField] private GameObject sphere;

        public Agent source;

        public float damage;
        public float areaRadius;

        public int ticks;
        public float tickDelay;

        public float knockbackForce;

        public bool canHitPlayer;
        public int frameDelay;

        // ========= PRIVATE VARIABLES ===========
        private bool canTick = true;

        public List<Agent> agentsInRange;

        private Vector3 knockbackVelocity;

        private int currentFrame;

        private void Start()
        {
            agentsInRange = new List<Agent>();
            transform.localScale *= areaRadius;
        }

        private void FixedUpdate()
        {
            if (canTick)
            {
                StartCoroutine(ExecuteTickNextFrameCo());
                //Debug.Log("Tick " + Time.frameCount);
            }
        }

        private void executeTick()
        {
            canTick = false;

            Debug.Log("Executed tick");

            HitEvent hitEvent = new HitEvent(source);
            hitEvent.baseDamage = damage;

            if(agentsInRange.Count > 0 )
            {
                foreach (Agent agent in agentsInRange)
                {
                    //Debug.Log("Hit agent: " + agent.transform.name);

                    if(damage > 0)
                        agent.health.Hurt(hitEvent);

                    if (knockbackForce > 0)
                    {
                        Vector3 agentPos = agent.transform.position + new Vector3(0, agent.transform.localScale.y / 2, 0);
                        knockbackVelocity = (agentPos - transform.position).normalized;
                        knockbackVelocity *= knockbackForce;
                        agent.OnKnockbackReceived.Invoke(knockbackVelocity);
                    }
                }
            }
        }

        IEnumerator WaitForNextTickCo()
        {
            yield return new WaitForSeconds(tickDelay);
            ticks--;
            if (ticks > 0) canTick = true;
            else Destroy(gameObject);
        }

        IEnumerator ExecuteTickNextFrameCo()
        {
            while(currentFrame < frameDelay)
            {
                currentFrame++;
                yield return null;
            }
            
            executeTick();
            StartCoroutine(WaitForNextTickCo());
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Enemy"))
            {
                if (other.TryGetComponent(out Agent enemy))
                {
                    agentsInRange.Add(enemy);
                    Debug.Log("Added enemy");
                }
                    
            }
            if(other.CompareTag("Player") && canHitPlayer)
            {
                if(other.TryGetComponent(out Agent player))
                {
                    //Debug.Log("Player added " + Time.frameCount);
                    agentsInRange.Add(player);
                }

            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                if (other.TryGetComponent(out Agent enemy))
                    agentsInRange.Remove(enemy);
            }
            if (other.CompareTag("Player"))
            {
                if (other.TryGetComponent(out Agent player))
                    agentsInRange.Remove(player);
            }
        }
    }
}
