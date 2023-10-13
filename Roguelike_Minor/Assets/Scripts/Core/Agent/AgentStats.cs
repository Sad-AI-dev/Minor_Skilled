using System;
using UnityEngine;

namespace Game.Core {
    [System.Serializable]
    public class AgentStats
    {
        //combat
        [Header("Combat")]
        public float maxHealth = 10f;
        public float baseDamage = 1;

        //money
        [Header("Money")]
        [SerializeField] private int heldMoney;
        public int Money { 
            get { return heldMoney; } 
            set {
                heldMoney = value;
                onMoneyChanged?.Invoke(heldMoney);
            }
        }
        public Action<int> onMoneyChanged;

        //cooldowns
        [Header("Cooldowns")]
        public float attackSpeed = 1;
        public float coolDownMultiplier = 1;

        //movement
        [Header("Movement")]
        public float walkSpeed;
        public float sprintSpeed;
    }
}
