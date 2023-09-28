using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.Data {
    [System.Serializable]
    public class BehaviourPool<T> where T : MonoBehaviour
    {
        private readonly List<T> pool;
        public GameObject behaviourTemplate;

        //ctor
        public BehaviourPool()
        {
            pool = new List<T>();
        }

        public T GetBehaviour()
        {
            foreach (T behaviour in pool)
            {
                if (behaviour.gameObject.activeSelf)
                {
                    return behaviour;
                }
            }
            //no behaviour available
            return AddObject();
        }

        private T AddObject()
        {
            T behaviour = Object.Instantiate(behaviourTemplate).GetComponent<T>();
            pool.Add(behaviour);
            return behaviour;
        }
    }
}