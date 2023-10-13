using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Enemy.Core
{
    public abstract class Tree : MonoBehaviour
    {
        private BT_Node root = null;

        protected virtual void Start()
        {
            root = SetupTree();
        }

        protected virtual void Update()
        {
            if(root != null)
            {
                root.Evaluate();
            }
        }
        protected abstract BT_Node SetupTree();
    }
}
