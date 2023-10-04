using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Game.Core.Data;

[CustomPropertyDrawer(typeof(WeightedChance<>))]
public class WeightedChanceDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("options"), label, true);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        EditorGUI.PropertyField(position, property.FindPropertyRelative("options"), label, true);

        EditorGUI.EndProperty();
    }
}
