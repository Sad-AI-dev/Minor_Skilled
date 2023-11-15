using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game {
    public class StatTracker : MonoBehaviour
    {
        [Header("Damage Stats")]
        public float damageDealt;
        public float damageTaken;
        [Space(10f)]
        public int enemiesKilled;
        [Space(10f)]
        public float mostDamageDealt;

        [Header("Healing Stats")]
        public float totalHealed;

        [Header("Money Stats")]
        public int totalMoneyCollected;
        //totalMoneySpent
        //totalPurchases

        //total run time
        //stages cleared

        //total items collected
        //total slots collected

        //vars
        private Agent player;

        private void Start()
        {
            player = GameStateManager.instance.player;
            //initialize events
            EventBus<AgentTakeDamageEvent>.AddListener(HandleAgentTakeDamage);
            EventBus<AgentHealEvent>.AddListener(HandleAgentHeal);
        }

        //================== Track Stats =======================
        private void HandleAgentTakeDamage(AgentTakeDamageEvent eventData)
        {
            if (eventData.hitEvent.source == player)
            {
                UpdateDealDamage(eventData.hitEvent);
            }
            else if (eventData.hitEvent.target.agent == player)
            {
                UpdateTakeDamage(eventData.hitEvent);
            }
        }

        private void HandleAgentHeal(AgentHealEvent eventData)
        {
            if (eventData.healEvent.target == player)
            {
                UpdateHeal(eventData.healEvent);
            }
        }

        private void HandleEnemyDeath(HitEvent hitEvent)
        {
            enemiesKilled++;
            totalMoneyCollected += hitEvent.target.agent.stats.Money;
        }

        //====== Deal Damage Stats ======
        private void UpdateDealDamage(HitEvent hitEvent)
        {
            //initialize hit event
            hitEvent.onDeath.AddListener(HandleEnemyDeath);
            //track damage
            float totalDamage = hitEvent.GetTotalDamage();
            //update stats
            damageDealt += totalDamage;
            if (totalDamage > mostDamageDealt) { mostDamageDealt = totalDamage; }
        }

        //====== Take Damage Stats =====
        private void UpdateTakeDamage(HitEvent hitEvent)
        {
            damageTaken += hitEvent.GetTotalDamage();
        }

        //==== Heal Stats =====
        private void UpdateHeal(HealEvent healEvent)
        {
            totalHealed += healEvent.GetTotalHeal();
        }

        //===== Handle Destroy =====
        private void OnDestroy()
        {
            EventBus<AgentTakeDamageEvent>.RemoveListener(HandleAgentTakeDamage);
            EventBus<AgentHealEvent>.RemoveListener(HandleAgentHeal);
        }
    }
}
