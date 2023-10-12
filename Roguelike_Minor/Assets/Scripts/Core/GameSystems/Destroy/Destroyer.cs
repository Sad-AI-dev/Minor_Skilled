using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.GameSystems {
    public class Destroyer : MonoBehaviour
    {
        public void DestroyObject()
        {
            Destroy(gameObject);
        }
    }
}
