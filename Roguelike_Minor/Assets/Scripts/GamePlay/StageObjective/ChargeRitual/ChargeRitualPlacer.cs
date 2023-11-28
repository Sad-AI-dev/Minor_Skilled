using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game {
    public class ChargeRitualPlacer : MonoBehaviour
    {
        private void Awake()
        {
            EventBus<ObjectiveSpawned>.Invoke(new ObjectiveSpawned { objective = gameObject });
        }
    }
}
