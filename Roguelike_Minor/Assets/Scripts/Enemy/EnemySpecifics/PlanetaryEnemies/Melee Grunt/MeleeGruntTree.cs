using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Game.Enemy.Core;
using Game.Core;

namespace Game.Enemy {
    public class MeleeGruntTree : Core.Tree
    {
        //General Variables
        [Header("General Variables")]
        public Agent agent;
        public NavMeshAgent navAgent;

        //Projectile Arch
        [Header("Projectile Variables")]
        [SerializeField] float projectileSpeed;

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

        [Header("Debug variables")]
        public bool melee;
        public bool semi;
        public bool ranged;

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
                        new MeleeGruntMeleeAttack(agent, navAgent),
                    }),
                    //If not in any range, Walk to target
                    new MeleeGruntWalkToTarget(transform, agent, navAgent)
                }
            );
            return root;
        }

        private void OnCollisionEnter(Collision collision)
        {
            //TODO: Make it only work on charge
            if (collision.transform.tag == "Player" && charging) {
                Debug.Log("hit player");
                OnHitPlayer?.Invoke(this);
            }
        }

        private void OnDrawGizmos()
        {
            if (melee)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, meleeAttackRange);
            }

            if (semi)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(transform.position, semiMeleeAttackRange);
            }

            if (ranged)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(transform.position, rangedAttackRange);
            }
        }
    }
}
