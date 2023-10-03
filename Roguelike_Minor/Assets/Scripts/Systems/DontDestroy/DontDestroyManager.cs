using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems {
    public class DontDestroyManager : MonoBehaviour
    {
        //==== singleton ====
        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
                //initialize list
                dontDestroyObjects = new List<GameObject>();
            }
        }

        public static DontDestroyManager instance;

        //vars
        private List<GameObject> dontDestroyObjects;

        public void Register(GameObject obj)
        {
            dontDestroyObjects.Add(obj);
            obj.transform.SetParent(null); //make sure object has no parent
            DontDestroyOnLoad(obj);
        }

        public void Reset()
        {
            //destroy all objects in dont destroy on load
            for (int i = dontDestroyObjects.Count - 1; i >= 0; i--)
            {
                Destroy(dontDestroyObjects[i]);
            }
            dontDestroyObjects.Clear(); //ready list reuse
        }
    }
}
