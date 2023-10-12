using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.GameSystems {
    public class DontDestroyAssigner : MonoBehaviour
    {
        private void Start()
        {
            DontDestroyManager.instance.Register(gameObject);
        }
    }
}
