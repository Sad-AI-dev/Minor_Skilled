using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core.GameSystems;

namespace Game.Player {
    public class PlayerSpawner : MonoBehaviour
    {
        private void Start()
        {
            //get player
            PlayerController player = GameStateManager.instance.player.GetComponent<PlayerController>();
            //choose destination
            Vector3 destination = transform.GetChild(Random.Range(0, transform.childCount)).position;
            //warp
            player.transform.position = new Vector3(75, 1, 55);
            player.ResetVelocity();
        }
    }
}
