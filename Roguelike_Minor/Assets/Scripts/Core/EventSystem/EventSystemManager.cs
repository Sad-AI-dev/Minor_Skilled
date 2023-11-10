using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class EventSystemManager : MonoBehaviour
    {
        private void Awake()
        {
            if (invoked) { Destroy(gameObject); }
            else 
            { 
                CreateEventSystem();
                invoked = true;
            }
        }
        private static bool invoked;

        [SerializeField] private GameObject eventSystemPrefab;

        //======== Initialize Event System =========
        private void CreateEventSystem()
        {
            Instantiate(eventSystemPrefab);
        }
    }
}
