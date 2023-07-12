using UnityEngine;
using UnityEditor;

using Lotl.StateMachine;

[CustomPropertyDrawer(typeof(Transition))]
public class Transition_Drawer : PropertyDrawer
{
    const float fieldIndentation = 5;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        label = EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, label);

        EditorGUI.BeginChangeCheck();

        // Get properties.
        SerializedProperty condition = property.FindPropertyRelative("condition");
        SerializedProperty stateTo = property.FindPropertyRelative("stateTo");

        // Calculate rects for properties and labels.
        Rect conditionRect = new(position);
        conditionRect.width /= 2;
        conditionRect.xMax -= fieldIndentation / 2;
        Rect conditionLabelRect = new(conditionRect);
        conditionLabelRect.width = 40;
        conditionRect.xMin = conditionLabelRect.xMax + fieldIndentation;

        Rect stateToRect = new(position);
        stateToRect.xMin = conditionRect.xMax + fieldIndentation;
        Rect stateToLabelRect = new(stateToRect);
        stateToLabelRect.width = 17;
        stateToRect.xMin = stateToLabelRect.xMax + fieldIndentation;

        // Store old indent level and set it to 0, the PrefixLabel takes care of it.
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Draw properties.
        EditorGUI.LabelField(conditionLabelRect, "Check");
        EditorGUI.PropertyField(conditionRect, condition, GUIContent.none);
        EditorGUI.LabelField(stateToLabelRect, "To");
        EditorGUI.PropertyField(stateToRect, stateTo, GUIContent.none);

        if (EditorGUI.EndChangeCheck())
            property.serializedObject.ApplyModifiedProperties();

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }
}
