using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core.GameSystems;

namespace Game {
    public class LootSpawner : MonoBehaviour
    {
        public ObjectSpawner spawner;

        private void Start()
        {
            GameStateManager.instance.lootSpawner = this;
        }
    }
}
