using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game.Enemy.Core
{
    public enum NodeState
    {
        RUNNING,
        SUCCESS,
        FAILURE
    }
    public class BT_Node
    {
        protected NodeState state;
        public BT_Node parent;
        public List<BT_Node> children = new List<BT_Node>();
        private Dictionary<string, object> DataContext = new Dictionary<string, object>();
        public BT_Node()
        {
            parent = null;
        }
        public BT_Node(List<BT_Node> children) 
        {
            foreach (BT_Node child in children)
            {
                Attach(child);
            }
        }    
        private void Attach(BT_Node node)
        {
            node.parent = this;
            children.Add(node);
        }
        public virtual NodeState Evaluate() => NodeState.FAILURE;
        public void SetData(string key, object value)
        {
            if (parent != null)
            {
                parent.SetData(key, value);
            }
            else
            {
                DataContext[key] = value;
            }
        }

        public object GetData(string key)
        {
            object value = null;
            if (DataContext.TryGetValue(key, out value))
            {
                return value;
            }

            BT_Node node = parent;
            while(node != null)
            {
                value = node.GetData(key);
                if(value != null)
                {
                    return value;
                }
                node = node.parent;
            }
            return null;
        }
        public bool ClearData(string key)
        {
            if (DataContext.ContainsKey(key))
            {
                DataContext.Remove(key);
                return true;
            }

            BT_Node node = parent;
            while (node != null)
            {
                bool cleared = node.ClearData(key);
                if (cleared)
                {
                    return true;
                }
                node = node.parent;
            }
            return false;
        }

        public void SetTarget(Transform target)
        {
            if (parent != null)
            {
                parent.SetTarget(target);
            }
            else
            {
                SetData("Target", target);
                EventBus<GameEndEvent>.AddListener(ClearTarget);
            }
        }
        public void ClearTarget(GameEndEvent eventData)
        {
            ClearData("Target");
            EventBus<GameEndEvent>.RemoveListener(ClearTarget);
        }

        public void SetDistanceToTarget(float distanceToTarget)
        {
            if (parent != null)
            {
                parent.SetDistanceToTarget(distanceToTarget);
            }
            else
            {
                SetData("DistanceToTarget", distanceToTarget);
            }
        }
    }
}
