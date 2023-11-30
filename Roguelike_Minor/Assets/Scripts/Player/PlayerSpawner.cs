using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player {
    public class PlayerSpawner : MonoBehaviour
    {
        private void Start()
        {
            //get player
            PlayerController player = GameStateManager.instance.player.GetComponent<PlayerController>();
            //choose destination
            Transform destination = transform.GetChild(Random.Range(0, transform.childCount));
            //warp
            player.transform.SetPositionAndRotation(destination.position, Quaternion.LookRotation(destination.forward));
            player.ResetVelocity();
            //reset cam
            player.GetComponent<CameraController>().ResetCamera(player.transform.rotation);
        }
    }
}
