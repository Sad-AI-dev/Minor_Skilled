using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Core {
    public class AgentHealthManager : MonoBehaviour, IHittable
    {
        [HideInInspector] public Agent agent;

        [Header("Health")]
        public float health;
        private float MaxHealth { get { return agent.stats.maxHealth; } }
        public HealthBar healthBar;

        [Header("Events")]
        public UnityEvent<HealEvent> onHeal;
        public UnityEvent<HitEvent> onHurt;
        public UnityEvent<HitEvent> onDeath;

        //vars
        private bool useHealthBar;

        private void Start()
        {
            useHealthBar = healthBar != null;
            health = MaxHealth;
        }

        //============ IHittable ===============
        public void Hurt(HitEvent hitEvent)
        {
            ProcessHitEvent(ref hitEvent);
            //take damage
            health -= hitEvent.GetTotalDamage();
            onHurt?.Invoke(hitEvent);
            if (health <= 0) { HandleDeath(ref hitEvent); }
            //update health bar
            HandleHealthChange();
        }

        public void Heal(HealEvent healEvent)
        {
            ProcessHealEvent(ref healEvent);
            //heal
            health += healEvent.GetTotalHeal();
            onHeal?.Invoke(healEvent);
            //update health bar
            HandleHealthChange();
        }

        //=============== Take Damage ================
        private void ProcessHitEvent(ref HitEvent hitEvent)
        {
            agent.effectHandler.ProcessHitEvent(ref hitEvent);
            //TODO: allow items to effect hitEvent
        }

        private void HandleDeath(ref HitEvent hitEvent)
        {
            //invoke death events
            hitEvent.onDeath?.Invoke(agent);
            onDeath?.Invoke(hitEvent);
        }

        //=================== Heal ===================
        private void ProcessHealEvent(ref HealEvent healEvent)
        {
            agent.effectHandler.ProcessHealEvent(ref healEvent);
            //TODO: allow items to effect healEvent
        }

        //=============== Generic Health Change ==============
        private void HandleHealthChange()
        {
            health = Mathf.Clamp(health, 0, MaxHealth); //clamp for healthBar visuals
            if (useHealthBar)
            {
                healthBar.UpdateHealthBar(health / MaxHealth);
            }
        }
    }
}
