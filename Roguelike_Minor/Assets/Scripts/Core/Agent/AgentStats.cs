using System;
using UnityEngine;

namespace Game.Core {
    [System.Serializable]
    public class AgentStats
    {
        //health
        [Header("Health")]
        public float maxHealth = 10f;
        public float maxHealthMult = 1f;
        [Tooltip("Expressed in health / second")]
        public float regeneration = 0f;
        //combat
        [Header("Combat")]
        public float baseDamage = 1;
        [Space(10f)]
        public float critChance = 1f;
        public float critMult = 2f;

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
        public int currentJumps;
        public int totalJumps;

        [Header("Luck")]
        public int luck;

        public void Copy(AgentStats other)
        {
            //health
            maxHealth = other.maxHealth;
            maxHealthMult = other.maxHealthMult;
            regeneration = other.regeneration;

            //combat
            baseDamage = other.baseDamage;
            critChance = other.critChance;
            critMult = other.critMult;

            //money
            Money = other.heldMoney;

            //cooldowns
            attackSpeed = other.attackSpeed;
            coolDownMultiplier = other.coolDownMultiplier;

            //movement
            walkSpeed = other.walkSpeed;
            sprintSpeed = other.sprintSpeed;
            currentJumps = other.currentJumps;
            totalJumps = other.totalJumps;

            //luck
            luck = other.luck;
        }

        //======== Health Getter ==========
        public float GetMaxHealth()
        {
            return maxHealth * maxHealthMult;
        }
    }
}
