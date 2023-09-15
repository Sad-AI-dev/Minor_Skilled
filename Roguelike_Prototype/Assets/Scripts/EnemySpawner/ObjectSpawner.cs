using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public bool spawnOnStart;
    [HideIf(nameof(notSpawnOnStart))]
    public GameObject objectToSpawnOnStart;

    public List<Transform> spawnPoints;

    //hide condition
    public bool notSpawnOnStart => !spawnOnStart;

    private void Awake()
    {
        if (spawnPoints == null || spawnPoints.Count == 0) {
            spawnPoints = new List<Transform>();
            foreach (Transform child in transform) {
                spawnPoints.Add(child);
            }
        }
    }

    private void Start()
    {
        if (spawnOnStart) { SpawnObject(objectToSpawnOnStart); }
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
