using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Game;

[CustomEditor(typeof(AdvanceObjectSpawner))]
public class AdvanceObjectSpawnerEditor : Editor
{
    private AdvanceObjectSpawner spawner;

    private void OnEnable()
    {
        if (!spawner) { spawner = target as AdvanceObjectSpawner; }
    }

    private void OnSceneGUI()
    {
        //draw debug ranges
        Handles.color = Color.red;
        Handles.DrawWireDisc(spawner.transform.position, Vector3.up, spawner.minDistance);
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(spawner.transform.position, Vector3.up, spawner.minDistance + spawner.range);
    }
}
