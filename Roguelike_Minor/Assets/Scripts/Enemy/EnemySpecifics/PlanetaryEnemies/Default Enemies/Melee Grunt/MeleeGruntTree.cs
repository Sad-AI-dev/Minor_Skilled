using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Game.Enemy.Core;
using Game.Core;

namespace Game.Enemy {
    public class MeleeGruntTree : Core.Tree
    {
        [Header("Knockback Variables")]
        public float upMultiplier;
        public float directionMultiplier;

        //Variables
        [Header("Attack Ranged")]
        public static float rangedAttackRange = 12;
        public static float semiMeleeAttackRange = 7;
        public static float meleeAttackRange = 1.6f;

        [Header("Charce Variables")]
        public int charceChancePercent = 15;
        public int chargeWindUpTimer = 2;

        public delegate void Notify(MeleeGruntTree source);
        public event Notify OnHitPlayer;
        public bool charging;

        protected override void Start()
        {
            navAgent.stoppingDistance = meleeAttackRange - 0.1f;
            base.Start();
        }

        protected override BT_Node SetupTree()
        {
            BT_Node root = new Selector(
                new List<BT_Node>
                {
                    //Choose ranged or Melee
                    new TakeKnockback(transform, agent, navAgent, rb, upMultiplier, directionMultiplier),
                    new MeleeGruntChooseAttack(transform),
                    new Sequence( new List<BT_Node>
                    {
                       //If chosen Ranged, handle ranged
                       new TaskCheckRanged(true),
                       new MeleeGruntHandleRanged(transform, rangedAttackRange, agent, navAgent)
                    }),//Works
                    new Sequence( new List<BT_Node>
                    {
                        //If chosen Melee, Check Semi
                        new TaskCheckSemi(false, charceChancePercent, agent),
                        new MeleeGruntHandleChargeChance(agent, navAgent, transform),
                    }),
                    new Sequence( new List<BT_Node>
                    {
                        //Else Melee
                        new TaskCheckMelee(false, agent),
                        new MeleeGruntMeleeAttack(agent, navAgent, transform),
                    }),
                    //If not in any range, Walk to target
                    new MeleeGruntWalkToTarget(transform, agent, navAgent)
                }
            );
            return root;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && charging)
            {
                OnHitPlayer?.Invoke(this);
            }
        }
    }
}