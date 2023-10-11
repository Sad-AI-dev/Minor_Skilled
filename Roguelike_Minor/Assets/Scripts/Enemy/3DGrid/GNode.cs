using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Enemy.Pathfinding
{
    public enum NodeType { ground, air }
    public class GNode
    {
        public int x;
        public int y;
        public int z;

        public float hcost;
        public float gcost;

        public float fcost
        {
            get
            {
                return gcost + hcost;
            }
        }

        public GNode parent;
        public bool walkable;

        public GameObject worldObject;
        public NodeType nodeType;
    }
}
