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

        [Header("Luck")]
        public int luck;

        public void Copy(AgentStats other)
        {
            //combat
            maxHealth = other.maxHealth;
            baseDamage = other.baseDamage;

            //money
            Money = other.heldMoney;

            //cooldowns
            attackSpeed = other.attackSpeed;
            coolDownMultiplier = other.coolDownMultiplier;

            //movement
            walkSpeed = other.walkSpeed;
            sprintSpeed = other.sprintSpeed;
        }
    }
}
