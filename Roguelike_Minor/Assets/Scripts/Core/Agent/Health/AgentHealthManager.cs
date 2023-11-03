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
        private float MaxHealth { get { return agent.stats.maxHealth * agent.stats.maxHealthMult; } }
        public HealthBar healthBar;

        [Header("Events")]
        [HideInInspector] public UnityEvent onMaxHealthChanged;
        public UnityEvent<HealEvent> onHeal;
        public UnityEvent<HitEvent> onHurt;
        public UnityEvent<HitEvent> onDeath;

        //vars
        private bool useHealthBar;

        private void Start()
        {
            useHealthBar = healthBar != null;
            health = MaxHealth;
            onMaxHealthChanged = new UnityEvent();
            onMaxHealthChanged.AddListener(HandleHealthChange);
        }

        //============ IHittable ===============
        public void Hurt(HitEvent hitEvent)
        {
            if (health <= 0f) { return; } //ignore if dead

            hitEvent.target = this;
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
            healEvent.target = this;
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
            if (hitEvent.hasAgentSource)
            {
                hitEvent.source.effectHandler.ProcessHitEvent(ref hitEvent);
                hitEvent.source.inventory.ProcessHitEvent(ref hitEvent);
            }
            agent.effectHandler.ProcessHitEvent(ref hitEvent);
            agent.inventory.ProcessHitEvent(ref hitEvent);
        }

        private void HandleDeath(ref HitEvent hitEvent)
        {
            //reward money
            RewardMoney(ref hitEvent);
            //invoke death events
            hitEvent.onDeath?.Invoke(hitEvent);
            onDeath?.Invoke(hitEvent);
        }

        //=============== Money ====================
        private void RewardMoney(ref HitEvent hitEvent)
        {
            if (hitEvent.hasAgentSource)
            {
                hitEvent.source.stats.Money += agent.stats.Money;
            }
        }

        //=================== Heal ===================
        private void ProcessHealEvent(ref HealEvent healEvent)
        {
            agent.effectHandler.ProcessHealEvent(ref healEvent);
            agent.inventory.ProcessHealEvent(ref healEvent);
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
