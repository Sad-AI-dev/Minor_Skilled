using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems {
    public class DontDestroyAssigner : MonoBehaviour
    {
        private void Start()
        {
            DontDestroyManager.instance.Register(gameObject);
        }
    }
}
