using UnityEngine;
using UnityEditor;
using Game.Core.Data;

[CustomPropertyDrawer(typeof(UnityDictionary<,>))]
public class UnityDictionaryDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("dictionary"), label, true);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        EditorGUI.PropertyField(position, property.FindPropertyRelative("dictionary"), label, true);

        EditorGUI.EndProperty();
    }
}
