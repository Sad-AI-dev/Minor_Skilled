using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Game;

[CustomEditor(typeof(EnemyPlacer))]
public class EnemyPlacerEditor : Editor
{
    private EnemyPlacer placer;

    private void OnEnable()
    {
        placer = target as EnemyPlacer;
    }

    private void OnSceneGUI()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(placer.transform.position, Vector3.up, placer.innerRingMin);
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(placer.transform.position, Vector3.up, placer.innerRingMin + placer.innerRingSize);
        Handles.color = Color.green;
        Handles.DrawWireDisc(placer.transform.position, Vector3.up, placer.innerRingMin + placer.innerRingSize + placer.outerRingSize);
    }
}
