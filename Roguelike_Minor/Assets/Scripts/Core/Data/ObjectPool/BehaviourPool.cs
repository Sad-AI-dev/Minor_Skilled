using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.Data {
    [System.Serializable]
    public class BehaviourPool<T> where T : MonoBehaviour
    {
        [SerializeField] private List<T> pool = new List<T>();
        public GameObject behaviourTemplate;

        //ctor
        public BehaviourPool(GameObject template = null)
        {
            pool = new List<T>();
            behaviourTemplate = template;
        }

        //============= Get Behaviour =================
        public T GetBehaviour()
        {
            foreach (T behaviour in pool)
            {
                if (!behaviour.gameObject.activeSelf)
                {
                    behaviour.gameObject.SetActive(true);
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

        //============== Reset Pool ==================
        public void Reset()
        {
            for (int i = 0; i < pool.Count; i++)
            {
                pool[i].gameObject.SetActive(false);
            }
        }

        public void DestroyReset()
        {
            for (int i = 0; i < pool.Count; i++)
            {
                Object.Destroy(pool[i].gameObject);
            }
            pool.Clear();
        }
    }
}
