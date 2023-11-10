using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

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
            player.transform.position = destination;
            player.ResetVelocity();
            //invoke bus event
            EventBus<PlayerWarpedEvent>.Invoke(new PlayerWarpedEvent() { 
                player = player.transform, 
                newPlayerPos = destination
            });
        }
    }
}
