using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game.Player {
    public class PlayerRespawner : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                EventBus<RespawnEvent>.Invoke(new RespawnEvent());
            }
        }
    }
}
