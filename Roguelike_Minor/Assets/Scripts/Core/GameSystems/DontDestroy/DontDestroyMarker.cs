using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.GameSystems {
    public class DontDestroyMarker : MonoBehaviour
    {
        private void Awake()
        {
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
    }
}
