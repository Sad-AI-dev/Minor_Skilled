using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Game.Core {
    public class AgentHealthManager : MonoBehaviour, IHittable
    {
        [HideInInspector] public Agent agent;

        [Header("Health")]
        public float health;
        public float regenDelay = 1f;
        private float MaxHealth { get { return agent.stats.maxHealth * agent.stats.maxHealthMult; } }
        public HealthBar healthBar;

        [Header("Events")]
        [HideInInspector] public UnityEvent onMaxHealthChanged;
        public UnityEvent<HealEvent> onHeal;
        public UnityEvent<HitEvent> onHurt;
        public UnityEvent<HitEvent> onDeath;

        //vars
        private bool useHealthBar;

        //regen vars
        private bool canRegen;
        private Coroutine pauseRegenRoutine;

        //event processors
        private List<IDealDamageProcessor> dealDamageProcessors;
        private List<ITakeDamageProcessor> takeDamageProcessors;
        private List<IHealProcessor> healProcessors;

        private void Awake()
        {
            InitializeProcessors();
        }
        private void Start()
        {
            useHealthBar = healthBar != null;
            health = MaxHealth;
            onMaxHealthChanged = new UnityEvent();
            onMaxHealthChanged.AddListener(HandleHealthChange);
            //Setup regen vars
            canRegen = true;
            StartCoroutine(RegenCo());
        }
        private void InitializeProcessors()
        {
            dealDamageProcessors = new List<IDealDamageProcessor>();
            takeDamageProcessors = new List<ITakeDamageProcessor>();
            healProcessors = new List<IHealProcessor>();
        }

        //============ IHittable ===============
        public void Hurt(HitEvent hitEvent)
        {
            if (health <= 0f) { return; } //ignore if dead

            hitEvent.target = this;
            if (hitEvent.hasAgentSource) { hitEvent.source.health.ProcessDealDamageEvent(ref hitEvent); }
            ProcessTakeDamageEvent(ref hitEvent);
            if (hitEvent.blocked) { return; } //hit was blocked, ignore
            //pause regen
            PauseRegen();
            //take damage
            health -= hitEvent.GetTotalDamage();
            onHurt?.Invoke(hitEvent);
            EventBus<AgentTakeDamageEvent>.Invoke(new AgentTakeDamageEvent() { hitEvent = hitEvent });
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
            EventBus<AgentHealEvent>.Invoke(new AgentHealEvent() { healEvent = healEvent });
            //update health bar
            HandleHealthChange();
        }

        //=============== Take Damage ================
        private void ProcessTakeDamageEvent(ref HitEvent hitEvent)
        {
            //process
            foreach (ITakeDamageProcessor processor in takeDamageProcessors)
            {
                ProcessTakeDamageProcessor(ref hitEvent, processor);
            }
        }
        private void ProcessDealDamageEvent(ref HitEvent hitEvent)
        {
            //process
            foreach (IDealDamageProcessor processor in dealDamageProcessors)
            {
                ProcessDealDamageProcessor(ref hitEvent, processor);
            }
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
            foreach (IHealProcessor processor in healProcessors)
            {
                ProcessHealEventProcessor(ref healEvent, processor);
            }
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

        //========= Passive Regeneration ===========
        private IEnumerator RegenCo()
        {
            while (true)
            {
                //regen health based on elapsed time
                if (canRegen) {
                    RegenHealth(agent.stats.regeneration * Time.deltaTime); }
                //wait for frame
                yield return null;
            }
        }

        private void RegenHealth(float toRegen)
        {
            health += toRegen;
            HandleHealthChange();
        }

        private void PauseRegen()
        {
            //stop existing routine
            if (pauseRegenRoutine != null) { StopCoroutine(pauseRegenRoutine); }
            //reset timer
            pauseRegenRoutine = StartCoroutine(PauseRegenCo());
        }
        private IEnumerator PauseRegenCo()
        {
            canRegen = false;
            yield return new WaitForSeconds(regenDelay);
            canRegen = true;
        }

        //================= Manage Processors ===================
        public void AddProcessor(IEventProcessor processor)
        {
            if (processor is IDealDamageProcessor) 
            { 
                dealDamageProcessors.Add(processor as IDealDamageProcessor);
                dealDamageProcessors.Sort((IDealDamageProcessor a, IDealDamageProcessor b) => a.CompareTo(b));
            }
            else if (processor is ITakeDamageProcessor) 
            { 
                takeDamageProcessors.Add(processor as ITakeDamageProcessor);
                takeDamageProcessors.Sort((ITakeDamageProcessor a, ITakeDamageProcessor b) => a.CompareTo(b));
            }
            else 
            { 
                healProcessors.Add(processor as IHealProcessor);
                healProcessors.Sort((IHealProcessor a, IHealProcessor b) => a.CompareTo(b));
            }
        }

        public void RemoveProcessor(IEventProcessor processor)
        {
            if (processor is IDealDamageProcessor) { dealDamageProcessors.Remove(processor as IDealDamageProcessor); }
            else if (processor is ITakeDamageProcessor) { takeDamageProcessors.Remove(processor as ITakeDamageProcessor); }
            else { healProcessors.Remove(processor as IHealProcessor); }
        }

        //================ Processors ========================
        private void ProcessDealDamageProcessor(ref HitEvent hitEvent, IDealDamageProcessor processor)
        {
            if (processor is ItemDataSO) { agent.inventory.ProcessDealDamage(ref hitEvent, processor); }
            //if not item, then must be status condition
            else { agent.effectHandler.ProcessDealDamage(ref hitEvent, processor); }
        }

        private void ProcessTakeDamageProcessor(ref HitEvent hitEvent, ITakeDamageProcessor processor)
        {
            if (processor is ItemDataSO) { agent.inventory.ProcessTakeDamage(ref hitEvent, processor); }
            //if not item, then must be status condition
            else { agent.effectHandler.ProcessTakeDamage(ref hitEvent, processor); }
        }

        private void ProcessHealEventProcessor(ref HealEvent healEvent, IHealProcessor processor)
        {
            if (processor is ItemDataSO) { agent.inventory.ProcessHealEvent(ref healEvent, processor); }
            //if not item, then must be status condition
            else { agent.effectHandler.ProcessHeal(ref healEvent, processor); }
        }
    }
}
