using System.Reflection;
using System.Collections;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomPropertyDrawer(typeof(HideIfAttribute))]
public class HideIfDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //get reference to attribute
        HideIfAttribute hideIf = attribute as HideIfAttribute;
        bool hidden = GetConditionalResult(hideIf, property);

        if (!hidden) {
            DrawDefaultField(position, property, label);
        }
    }
    private void DrawDefaultField(Rect position, SerializedProperty property, GUIContent label)
    {
        if (IsEventType()) {
            new UnityEventDrawer().OnGUI(position, property, label); //use internal UnityEventDrawer
        }
        else {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }

    //========================= Should property be hidden? ======================================
    private bool GetConditionalResult(HideIfAttribute hideIf, SerializedProperty property)
    {
        bool result;

        //look for conditional field
        string conditionalPath = property.propertyPath.Contains(".") ?
            System.IO.Path.ChangeExtension(property.propertyPath, hideIf.conditionalField) :
            hideIf.conditionalField;

        SerializedProperty conditionalProperty = property.serializedObject.FindProperty(conditionalPath);

        if (conditionalProperty != null) {
            result = conditionalProperty.boolValue;
        }
        else {
            object propertyParent = GetParentObject(property);
            result = (bool)(propertyParent.GetType().GetProperty(hideIf.conditionalField))
                .GetValue(propertyParent, new object[0]);
        }
        return result;
    }

    private object GetParentObject(SerializedProperty property)
    {
        string[] path = property.propertyPath.Split('.');

        object propertyObject = property.serializedObject.targetObject;
        object propertyParent = null;

        for (int i = 0; i < path.Length; i++) {
            if (path[i] == "Array") {
                int index = (int)(path[i + 1][path[i + 1].Length - 2] - '0');
                propertyObject = ((IList)propertyObject)[index];
                i++;
            }
            else {
                propertyParent = propertyObject;
                FieldInfo objInfo = propertyObject.GetType().GetField(path[i], 
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic); //internet told me this fixes things... Seems to fix hidden fields return null in this function
                if (objInfo != null) { //make sure field exists
                    propertyObject = objInfo.GetValue(propertyObject);
                }
            }
        }
        return propertyParent;
    }

    //=========================== Property Height =======================
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        HideIfAttribute hideIf = attribute as HideIfAttribute;
        bool hidden = GetConditionalResult(hideIf, property);

        if (!hidden) {
            return GetDefaultPropertyHeight(property, label);
        }
        else {
            //property is not drawn
            //remove space added before and after the property
            return -EditorGUIUtility.standardVerticalSpacing;
        }
    }

    private float GetDefaultPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (IsEventType()) {
            return new UnityEventDrawer().GetPropertyHeight(property, label);
        }
        else {
            return EditorGUI.GetPropertyHeight(property, label);
        }
    }

    //================= Generic Helper Funcs =======================
    private bool IsEventType()
    {
        return fieldInfo.FieldType == typeof(UnityEngine.Events.UnityEvent) ||
            ( fieldInfo.FieldType.IsGenericType &&
            fieldInfo.FieldType.GetGenericTypeDefinition() == typeof(UnityEngine.Events.UnityEvent<>));
    }
}
