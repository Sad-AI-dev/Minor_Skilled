using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Game.Enemy.Core;
using Game.Core;

namespace Game.Enemy {
    public class BigSquidTree : Core.Tree
    {
        public static int FireRangeMin = 30;
        public static int FireRangeMax = 40;
        public static float MinimumHeight = 10;

        [Header("Small Squid Specific Variables")]
        public LineRenderer lineRenderer;

        BT_Node root;

        protected override BT_Node SetupTree()
        {
            root = new Selector(
                new List<BT_Node>
                {
                    //Check if in shooting range
                    //handle shooting
                    new Sequence(new List<BT_Node>{
                        new BigSquidRangeCheck(transform),
                        new BigSquidHandleAttack(transform, agent, lineRenderer, rb)
                    }),
                    //Get Path to player and follow path
                    new BigSquidMoveToTarget(transform, agent, rb)

                }
            ) ;
            return root;
        }


        protected void FixedUpdate()
        {
            if (root != null)
            {
                if(root.GetData("MoveDirection") != null) rb.MovePosition(transform.position + (Vector3)root.GetData("MoveDirection") * Time.fixedDeltaTime);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, FireRangeMax);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, FireRangeMin);


        }
    }
}
