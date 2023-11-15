using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Game.Enemy.Core;
using Game.Core;

namespace Game.Enemy {
    public class SmallSquidTree : Core.Tree
    {
        public static int FlightPatrolRange = 20;
        public static int AttackRange = 50;
        public static float ExplosionRange = 1.2f;
        public static float ExplosionTime = 0.5f;

        [Header("Small Squid Specific Variables")]
        public int flightPatrolRange = 20;
        public int attackRange = 50;
        public float explosionRange = 1.2f;
        public float explosionTime = 0.5f;

        public GameObject ExplosionVisuals;
        public LayerMask playerLayerMask;

        BT_Node root;

        protected override void Start()
        {
            base.Start();
            FlightPatrolRange = flightPatrolRange;
            AttackRange = attackRange;
            ExplosionRange = explosionRange;
            ExplosionTime = explosionTime;
        }

        protected override BT_Node SetupTree()
        {
            root = new Selector(
                new List<BT_Node>
                {
                    new Sequence(new List<BT_Node>{
                        //Check target in range
                        new SmallSquidCheckRange(transform, agent),
                        //Dive and Explode when in range
                        new SmallSquidMoveToTarget(transform, agent, playerLayerMask, ExplosionVisuals, rb)
                    }),
                    //Otherwise fly in the sky between random points
                    new SmallSquidMoveThroughSky(transform, agent, rb)
                }
            );
            return root;
        }

        protected void FixedUpdate()
        {
            if (root != null)
            {
                if (root.GetData("MoveDirection") != null)
                {
                    rb.MovePosition((Vector3)root.GetData("MoveDirection"));
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, FlightPatrolRange);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, AttackRange);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, ExplosionRange);
        }
    }
}
