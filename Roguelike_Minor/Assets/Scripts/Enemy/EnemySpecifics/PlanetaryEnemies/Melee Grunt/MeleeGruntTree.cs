using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using Game.Enemy.Core;
using Game.Core;

namespace Game.Enemy
{
    public class MeleeGruntTree : Tree
    {
        public Agent agent;
        public NavMeshAgent navAgent;

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

                    }), 
                    new Sequence( new List<BT_Node>
                    {
                        //If chosen Melee, handle Melee

                    }),
                    //If not in any range, Walk to target

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
    }
}
