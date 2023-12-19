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

        [Header("Big Squid Specific Variables")]
        public LineRenderer lineRenderer;
        public float rotationSpeedMoving = 100;
        public float rotationSpeedTargeting = 60;

        protected override void Start()
        {
            agent.abilities.primary.vars = new BigSquidPrimaryVars
            {
                target = null,
                lineRenderer = this.lineRenderer,
                targetingCo = null,
                root = this.root
            };
            base.Start();
        }

        protected override BT_Node SetupTree()
        {
            root = new Selector(
                new List<BT_Node>
                {
                    //handle shooting
                    new Sequence(new List<BT_Node>{
                        new BigSquidRangeCheck(transform, Random.Range(FireRangeMin, FireRangeMax), agent),
                        new BigSquidHandleAttack(transform, agent, lineRenderer, rb, rotationSpeedTargeting)
                    }),
                    //Get Path to player and follow path
                    new BigSquidMoveToTarget(transform, agent, rb, rotationSpeedMoving)

                }
            ) ;
            return root;
        }


        protected void FixedUpdate()
        {
            if (root != null)
            {
                if (root.GetData("MoveDirection") != null) rb.MovePosition(transform.position + (Vector3)root.GetData("MoveDirection") * Time.fixedDeltaTime);
                if (root.GetData("MoveRotation") != null) rb.MoveRotation((Quaternion)root.GetData("MoveRotation"));
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
