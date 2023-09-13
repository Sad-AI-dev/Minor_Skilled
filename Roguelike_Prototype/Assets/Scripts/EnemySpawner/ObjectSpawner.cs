using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public List<Transform> spawnPoints;

    private void Awake()
    {
        if (spawnPoints == null || spawnPoints.Count == 0) {
            spawnPoints = new List<Transform>();
            foreach (Transform child in transform) {
                spawnPoints.Add(child);
            }
        }
    }

    public void SpawnObject(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab);
        obj.transform.SetPositionAndRotation(GetRandomSpawnPoint(), GetRandomYRotation());
    }

    private Vector3 GetRandomSpawnPoint()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Count)].position;
    }
    private Quaternion GetRandomYRotation()
    {
        return Quaternion.Euler(0, Random.Range(0, 360), 0);
    }
}
