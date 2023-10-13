using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Enemy.Pathfinding
{
    public enum NodeType { ground, air }
    [System.Serializable]
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

        public Vector3 worldPosition;
        public NodeType nodeType;
    }
}
