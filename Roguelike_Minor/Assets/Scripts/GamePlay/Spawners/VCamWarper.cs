using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Game.Core;

namespace Game {
    public class VCamWarper : MonoBehaviour
    {
        [SerializeField] private CinemachineFreeLook vCam;

        //=========== Start ===========
        private void Start()
        {
            EventBus<PlayerWarpedEvent>.AddListener(HandleSceneLoad);
        }

        //=========== Handle Warp ===========
        private void HandleSceneLoad(PlayerWarpedEvent eventData)
        {
            StartCoroutine(WarpCo(eventData.player, eventData.newPlayerPos));
        }

        private IEnumerator WarpCo(Transform player, Vector3 newPos)
        {
            yield return null;
            vCam.OnTargetObjectWarped(player, newPos);
            vCam.Follow = player;
        }

        //=========== Destroy ===========
        private void OnDestroy()
        {
            EventBus<PlayerWarpedEvent>.RemoveListener(HandleSceneLoad);
        }
    }
}
