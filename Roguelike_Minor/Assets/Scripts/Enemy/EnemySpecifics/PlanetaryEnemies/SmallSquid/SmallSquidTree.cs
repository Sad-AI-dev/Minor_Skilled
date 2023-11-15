using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Game.Enemy.Core;
using Game.Core;

namespace Game.Enemy {
    public class SmallSquidTree : Core.Tree
    {
        public static int FlightPatrolRange = 15;
        public static int AttackRange = 40;
        public static float ExplosionRange = 1.2f;

        [Header("Small Squid Specific Variables")]
        public GameObject ExplosionVisuals;
        public LayerMask playerLayerMask;

        protected override BT_Node SetupTree()
        {
            BT_Node root = new Selector(
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
