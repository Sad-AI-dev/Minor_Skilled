using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using UnityEngine.AI;

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

        public Transform transform;
        public Transform target;
        public Agent agent;
        public NavMeshAgent navAgent;

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
        
        //Handle Data sharing between nodes
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

        //Handle Setting Target
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

        //Handle Distance checks
        public IEnumerator DistanceToTargetCO(Agent agent, Transform transform, Transform target)
        {
            if (GetData("Target") != null)
            {
                float distance = Vector3.Distance(transform.position, target.position);
                SetDistanceToTarget(distance);
                yield return new WaitForSeconds(0.1f);
                agent.StartCoroutine(DistanceToTargetCO(agent, transform, target));
            }
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
