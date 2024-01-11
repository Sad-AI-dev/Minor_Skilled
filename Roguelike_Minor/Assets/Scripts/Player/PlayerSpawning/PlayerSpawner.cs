using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game.Player {
    public class PlayerSpawner : MonoBehaviour
    {
        private void Awake()
        {
            EventBus<RespawnEvent>.AddListener(HandleRespawn);
        }

        private void Start()
        {
            //get player
            PlayerController player = GameStateManager.instance.player.GetComponent<PlayerController>();
            //choose destination
            List<Transform> points = CompileValidPoints();
            Transform destination = points[Random.Range(0, points.Count)];
            //warp
            player.transform.SetPositionAndRotation(destination.position, Quaternion.LookRotation(destination.forward));
            player.ResetVelocity();
            //reset cam
            player.GetComponent<CameraController>().ResetCamera(player.transform.rotation);
        }

        private List<Transform> CompileValidPoints()
        {
            List<Transform> points = new List<Transform>();
            //compile child points
            foreach (Transform child in transform)
            {
                if (child.gameObject.activeSelf) { points.Add(child); }
            }
            //return result
            return points;
        }

        //============= Respawn Logic ================
        private void HandleRespawn(RespawnEvent eventData)
        {
            PlayerController player = GameStateManager.instance.player.GetComponent<PlayerController>();
            Transform closestPoint = GetClosestPoint(player.transform.position);
            //set player
            player.transform.SetPositionAndRotation(closestPoint.position, closestPoint.rotation);
            player.ResetVelocity();
        }

        private Transform GetClosestPoint(Vector3 playerPos)
        {
            Transform closest = transform.GetChild(0);
            float minDistance = Vector3.Distance(playerPos, closest.position);
            //search for closest point
            for (int i = 1; i < transform.childCount; i++)
            {
                float dist = Vector3.Distance(playerPos, transform.GetChild(i).position);
                if (dist < minDistance)
                {
                    minDistance = dist;
                    closest = transform.GetChild(i);
                }
            }
            //return result
            return closest;
        }

        //====== Handle Destroy =======
        private void OnDestroy()
        {
            EventBus<RespawnEvent>.RemoveListener(HandleRespawn);
        }
    }
}
