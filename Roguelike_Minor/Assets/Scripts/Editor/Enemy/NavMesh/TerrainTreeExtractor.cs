using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Game.Enemy;

[CustomEditor(typeof(NavMeshTerrainTreeHandling))]
public class TerrainTreeExtractor : Editor
{
    public override void OnInspectorGUI()
    {
        NavMeshTerrainTreeHandling script = (NavMeshTerrainTreeHandling)target;
        DrawDefaultInspector();
        if (GUILayout.Button("Extract"))
        {
            script.Extract();
        }
    }
}
