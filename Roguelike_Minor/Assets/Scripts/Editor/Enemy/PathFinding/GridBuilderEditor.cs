using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using Game.Enemy.Pathfinding;

[CustomEditor(typeof(GridBuilder))]
public class GridBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GridBuilder script = (GridBuilder)target;
        DrawDefaultInspector();
        if (GUILayout.Button("BuildGrid"))
        {
            script.BuildGrid();
        }
    }
}
