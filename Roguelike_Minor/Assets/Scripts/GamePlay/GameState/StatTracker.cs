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
        public int totalMoneySpent;
        public int totalPurchases;

        [Header("Progress Stats")]
        public int totalStagesCleared;
        public int totalShopVisits;
        public float runTime { get { return UITimeManager.currentTime; } }

        [Header("Inventory Stats")]
        public int totalItemsCollected;
        public int totalSlotsCollected { get { return CalcTotalSlots(); } }
        [Space(10f)]
        [SerializeField] private ItemDataSO slotPieceSO;
        [SerializeField] private int startSlots = 8;

        //vars
        private Agent player;

        private void Start()
        {
            player = GameStateManager.instance.player;
            //initialize events
            EventBus<AgentTakeDamageEvent>.AddListener(HandleAgentTakeDamage);
            EventBus<AgentHealEvent>.AddListener(HandleAgentHeal);
            EventBus<PurchaseEvent>.AddListener(HandlePurchase);
            EventBus<SceneLoadedEvent>.AddListener(HandleStageLoad);
            EventBus<PickupItemEvent>.AddListener(HandleItemPickup);
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
            if (eventData.healEvent.target.agent == player)
            {
                UpdateHeal(eventData.healEvent);
            }
        }

        private void HandleEnemyDeath(HitEvent hitEvent)
        {
            enemiesKilled++;
            totalMoneyCollected += hitEvent.target.agent.stats.Money;
        }

        private void HandlePurchase(PurchaseEvent eventData)
        {
            UpdatePurchase(eventData.price);
        }

        private void HandleStageLoad(SceneLoadedEvent eventData)
        {
            UpdateStageProgress();
        }

        private void HandleItemPickup(PickupItemEvent eventData)
        {
            UpdatePickupStats(eventData.item);
        }

        //================== Update Stats =======================
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

        private void UpdatePurchase(int price)
        {
            totalMoneySpent += price;
            totalPurchases++;
        }

        //==== stage progress stats ====
        private void UpdateStageProgress()
        {
            if (!GameStateManager.instance.scalingIsPaused) 
            { 
                totalStagesCleared++; 
            }
            else
            {
                totalShopVisits++;
            }
        }

        //=== Inventory stats ===
        private void UpdatePickupStats(ItemDataSO item)
        {
            if (item != slotPieceSO)
            {
                totalItemsCollected++;
            }
        }
        private int CalcTotalSlots()
        {
            SlotInventory slotInventory = player.inventory as SlotInventory;
            return slotInventory.slots - startSlots;
        }

        //===== Handle Destroy =====
        private void OnDestroy()
        {
            EventBus<AgentTakeDamageEvent>.RemoveListener(HandleAgentTakeDamage);
            EventBus<AgentHealEvent>.RemoveListener(HandleAgentHeal);
            EventBus<PurchaseEvent>.RemoveListener(HandlePurchase);
            EventBus<SceneLoadedEvent>.RemoveListener(HandleStageLoad);
            EventBus<PickupItemEvent>.RemoveListener(HandleItemPickup);
        }
    }
}
