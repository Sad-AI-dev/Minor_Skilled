using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Game.Enemy.Core;
using Game.Core;
using Game.Core.Data;

namespace Game.Enemy {
    public class Boss_BigSquidTree : Core.Tree
    {
        public static float AttackRange = 80;
        public static float RandomMoveArea = 40;
        public static int MaxY = 100;
        public static int MinimumHeight = 20;

        [Header("Boss Big Squid Specific Variables")]
        public float attackRange = 80;
        public float randomMoveArea = 40;
        public int maxY = 100;
        public int minimumHeight = 20;

        [Header("Minion Spawn Variables")]
        public WeightedChance<GameObject> weightedChance;
        [Tooltip("The chance of the boss creating a small or big squid")]
        public int spawnChancePercent = 20;
        [Tooltip("The amount of seconds between spawn chance check")]
        public float spawnChanceCooldown = 3;
        public Vector2Int spawnAmountMinMax = new Vector2Int(3, 7);

        [Header("Goop drop Variables")]
        public int goopDropChance = 20;
        public float goopChanceCooldown = 3;

        [Header("Shotgun Variables")]
        public int shotgunChance = 20;
        public float shotgunCooldownTime = 3;

        [Header("Debug variables")]
        [SerializeField] Transform target = null;
        [SerializeField] float distanceToCurrentTarget = 0;
        [SerializeField] bool patroling = false;

        protected override void Start()
        {
            AttackRange = attackRange;
            RandomMoveArea = randomMoveArea;
            MaxY = maxY;
            MinimumHeight = minimumHeight;
            base.Start();
        }

        protected override BT_Node SetupTree()
        {
            root = new Sequence(
                new List<BT_Node>
                {
                    new TakeKnockback(transform, agent, rb, 1, 2),
                    //Check if want to spawn random small or big squid
                    new Boss_BigSquid_SpawnMinionCheck(agent, transform, spawnChancePercent, weightedChance, spawnChanceCooldown, spawnAmountMinMax),
                    //Check if in range or if moving to target
                    new Selector(
                        new List<BT_Node>
                        {
                            //Check if in attack range
                            new Sequence( new List<BT_Node>{
                                    new Boss_BigSquid_CheckRange(transform),
                                    //If so keep moving randomly within attack range but to handle attacks
                                    new Boss_BigSquidFlyAroundTarget(agent, transform),
                                    //Drop goop
                                    new Boss_BigSquid_DropDOTChance(transform, agent, goopDropChance, goopChanceCooldown),
                                    new Boss_BigSquid_ShotgunChance(transform, agent, shotgunChance, shotgunCooldownTime)
                                }),
                            //If not in attack range move closer;
                            new Boss_BigSquid_MoveToTarget(transform, agent, rb)
                        }
                    )
                });

            return root;
        }

        private void FixedUpdate()
        {
            if (root != null && (root.GetData("TakingKnockback") == null || !(bool)root.GetData("TakingKnockback")))
            {
                if (root.GetData("MoveDirection") != null)
                {
                    rb.MovePosition(transform.position + (Vector3)root.GetData("MoveDirection") * Time.fixedDeltaTime);
                }
                if (root.GetData("TargetRotation") != null)
                {
                    rb.MoveRotation((Quaternion)root.GetData("TargetRotation"));
                }
            }

        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, randomMoveArea);
        }
    }
}
