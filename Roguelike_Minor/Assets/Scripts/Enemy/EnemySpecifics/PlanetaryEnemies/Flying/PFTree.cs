using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Enemy.Core;

namespace Game.Enemy {
    public class PFTree : Core.Tree
    {
        public static float FlightSpeed = 5;
        public static float AttackRange = 20;
        public static Queue<Vector3> path;

        protected override BT_Node SetupTree()
        {
            //Root
            BT_Node root = new Selector(
                new List<BT_Node> 
                {
                    //attack if in range
                    new Sequence(new List<BT_Node>
                    {
                        new PFCheckEnemyInRange(transform),
                        new PFAttackEnemy(transform)
                    }),
                    //else Fly to player.
                    new PFTaskFlyToEnemy(transform),
                }
            );
 
            return root;
        }

        protected override void Start()
        {
            base.Start();
        }
        protected override void Update()
        {
            base.Update();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, AttackRange);

            if(path != null && path.Count > 0)
            {
                foreach (var node in path)
                {
                    Gizmos.DrawSphere(node, 1);
                }
            }
        }
    }
}
