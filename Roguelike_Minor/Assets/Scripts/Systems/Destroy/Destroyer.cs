using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems {
    public class Destroyer : MonoBehaviour
    {
        public void DestroyObject()
        {
            Destroy(gameObject);
        }
    }
}
