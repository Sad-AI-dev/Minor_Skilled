using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game.Systems {
    public class GameStateManager : MonoBehaviour
    {
        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }
        }
        public static GameStateManager instance;

        //static ref to player
        public Agent player;
    }
}
