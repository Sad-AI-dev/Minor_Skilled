using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems {
    public class PlayerSpawner : MonoBehaviour
    {
        private void Start()
        {
            //reset player velocity

            //set player to random child position
            GameStateManager.instance.player.transform.position =
                transform.GetChild(Random.Range(0, transform.childCount)).position;
        }
    }
}
