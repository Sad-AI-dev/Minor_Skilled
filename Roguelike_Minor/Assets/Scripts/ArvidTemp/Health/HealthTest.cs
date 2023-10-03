using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game {
    public class HealthTest : MonoBehaviour
    {
        public Agent agent;
        public float damage = 1f;
        public float toHeal = 1f;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                HurtAgent();
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                HealAgent();
            }
        }

        private void HurtAgent()
        {
            HitEvent hitEvent = new HitEvent();
            hitEvent.baseDamage = damage;
            agent.health.Hurt(hitEvent);
        }

        private void HealAgent()
        {
            HealEvent healEvent = new HealEvent(toHeal, null);
            agent.health.Heal(healEvent);
        }
    }
}
