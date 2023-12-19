using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Game.Enemy.Core;
using Game.Core;

namespace Game.Enemy {
    public class MeleeGruntTree : Core.Tree
    {
        [Header("Attack variables")]
        public Animator anim;
        public GameObject PrimaryPrefab;

        [Header("Knockback Variables")]
        public float upMultiplier;
        public float directionMultiplier;

        //Variables
        [Header("Attack Ranged")]
        public float rangedAttackRange = 12;
        public float semiMeleeAttackRange = 7;
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
            agent.abilities.primary.vars = new PGPrimaryAttackVars
            {
                anim = this.anim,
                prefab = PrimaryPrefab
            };

            base.Start();
        }

        protected override BT_Node SetupTree()
        {
            root = new Selector(
                new List<BT_Node>
                {
                    //Choose ranged or Melee
                    new TakeKnockback(transform, agent, navAgent, rb, upMultiplier, directionMultiplier),
                    new MeleeGruntChooseAttack(transform),

                    //Handle Ranged attack
                    new Sequence( new List<BT_Node>
                    {
                       //If chosen Ranged, handle ranged
                       new TaskCheckRanged(transform, Random.Range(rangedAttackRange, rangedAttackRange/2), true),
                       new MeleeGruntHandleRanged(transform, rangedAttackRange, agent, navAgent)
                    }),
                    //Handle charge chance
                    new Sequence( new List<BT_Node>
                    {
                        new TaskCheckSemi(transform, semiMeleeAttackRange, false, charceChancePercent, agent),
                        new MeleeGruntHandleChargeChance(agent, navAgent, transform),
                    }),
                    //Handle melee attack
                    new Sequence( new List<BT_Node>
                    {
                        new TaskCheckMelee(transform, meleeAttackRange, false, agent),
                        new MeleeGruntMeleeAttack(agent, navAgent, transform),
                    }),
                    //Handle moving to target
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
