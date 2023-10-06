using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Player;

namespace Game.Systems {
    public class PlayerSpawner : MonoBehaviour
    {
        private void Start()
        {
            //set player to random child position
            GameStateManager.instance.player.transform.position =
                transform.GetChild(Random.Range(0, transform.childCount)).position;
            //reset player velocity
            GameStateManager.instance.player.GetComponent<PlayerController>().ResetVelocity();
        }
    }
}
