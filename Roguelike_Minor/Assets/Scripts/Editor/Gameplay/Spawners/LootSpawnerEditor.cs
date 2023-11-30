using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Game;

[CustomEditor(typeof(LootSpawner))]
public class LootSpawnerEditor : Editor
{
    private LootSpawner spawner;

    private void OnEnable()
    {
        if (!spawner)
        {
            spawner = target as LootSpawner;
        }
    }

    private void OnSceneGUI()
    {
        //draw debug range
        Handles.color = Color.green;
        Handles.DrawWireDisc(spawner.transform.position, Vector3.up, spawner.stageRange);
    }
}
