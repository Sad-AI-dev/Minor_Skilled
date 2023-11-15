using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Game.Enemy.Core;
using Game.Core;

namespace Game.Enemy {
    public class BigSquidTree : Core.Tree
    {
        public static int FireRange = 40;
        public static float MinimumHeight = 10;

        [Header("Small Squid Specific Variables")]
        public Transform EyePoint;
        public LineRenderer lineRenderer;

        protected override BT_Node SetupTree()
        {
            BT_Node root = new Selector(
                new List<BT_Node>
                {
                    //Check if in shooting range
                    //handle shooting
                    new Sequence(new List<BT_Node>{
                        new BigSquidRangeCheck(transform),
                        new BigSquidHandleAttack(agent, lineRenderer, EyePoint)
                    }),
                    //Get Path to player and follow path
                    new BigSquidMoveToTarget(transform, agent, rb)

                }
            ) ;
            return root;
        }


        private void OnDrawGizmosSelected()
        {
           
        }
    }
}
