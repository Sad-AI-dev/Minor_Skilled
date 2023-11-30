using Codice.Client.BaseCommands.CheckIn.Progress;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.GameSystems
{
    [RequireComponent(typeof(Rigidbody))]
    public class SeekingMissile : MonoBehaviour
    {
        private Rigidbody rb;

        private HitEvent hitEvent;
        private Agent sourceAgent;
        private Agent target;

        private float speed = 35;
        private Quaternion lookAtEnemy;
        private float turnSpeed = 0.2f;
        
        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.useGravity = false;
            transform.rotation = Quaternion.LookRotation(Vector3.up);
            target = FindEnemy();
        }

        private void FixedUpdate()
        {
            if (target == null)
                target = FindEnemy();

            Vector3 targetDir = (target.transform.position + Vector3.up) - transform.position;
            lookAtEnemy = Quaternion.LookRotation(targetDir);

            transform.rotation = Quaternion.Lerp(transform.rotation, lookAtEnemy, turnSpeed);

            rb.velocity = transform.forward * speed;
        }

        public void InitializeVars(HitEvent hitEvent)
        {
            if(hitEvent.hasAgentSource)
                sourceAgent = hitEvent.source;

            this.hitEvent = hitEvent;
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.tag);
            if (other.CompareTag(sourceAgent.tag)) return;
            
            if(other.TryGetComponent<Agent>(out Agent agent))
            {
                agent.health.Hurt(hitEvent);
            }

            Destroy(gameObject);
        }

        private Agent FindEnemy()
        {
            List<Agent> agents = new List<Agent>();
            float radius = 20;

            while (agents.Count <= 0)
            {
                agents = Explosion.FindAgentsInRange(sourceAgent.transform.position, radius, hitEvent.source);
                radius += 10;
            }

            return agents[0];
        }
    }
}
