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
        public static float rangedAttackRange;
        public static float semiMeleeAttackRange;
        public static float meleeAttackRange;

        [Header("Charce Variables")]
        public int charceChancePercent = 15;
        public int chargeWindUpTimer = 2;

        public delegate void Notify(MeleeGruntTree source);
        public event Notify OnHitPlayer;
        
        protected override BT_Node SetupTree()
        {
            BT_Node root = new Selector(
                new List<BT_Node>
                {
                    //Choose ranged or Melee
                    new MeleeGruntChooseAttack(),
                    new Sequence( new List<BT_Node>
                    {
                       //If chosen Ranged, handle ranged
                       new TaskCheckRanged(true),
                       new MeleeGruntHandleRanged(transform, rangedAttackRange, agent, navAgent)
                    }),
                    new Sequence( new List<BT_Node>
                    {
                        //If chosen Melee, Check Semi
                        new TaskCheckSemi(false, charceChancePercent),
                        new MeleeGruntHandleChargeChance(agent, navAgent, transform),
                    }),
                    new Sequence( new List<BT_Node>
                    {
                        //Else Melee
                        new TaskCheckMelee(false),
                        new MeleeGruntMeleeAttack(agent),
                    }),
                    //If not in any range, Walk to target
                    new MeleeGruntWalkToTarget(agent, navAgent)
                }
            );
            return root;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.tag == "Player") {
                OnHitPlayer?.Invoke(this);
            }
        }
    }
}
