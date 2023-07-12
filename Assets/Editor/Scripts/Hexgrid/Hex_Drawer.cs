using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using Lotl.Hexgrid;

[CustomPropertyDrawer(typeof(Hex))]
public class Hex_Drawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        label = EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, label);

        EditorGUI.BeginChangeCheck();

        // Get properties.
        SerializedProperty qAxis = property.FindPropertyRelative("q");
        SerializedProperty rAxis = property.FindPropertyRelative("r");


        // Calculate rects for properties and labels.
        float spacing = 10f, halfSpacing = spacing / 2;
        Rect qRect = new(position);
        qRect.width /= 3;
        
        Rect rRect = new(position);
        rRect.x = qRect.xMax;
        rRect.width /= 3;
        
        qRect.width -= halfSpacing;
        rRect.width -= halfSpacing;

        Rect qLabelRect = new(qRect);
        qRect.xMin = qLabelRect.xMax
            = qLabelRect.xMin + spacing;
        qRect.xMin += halfSpacing;

        Rect rLabelRect = new(rRect);
        rRect.xMin = rLabelRect.xMax
            = rLabelRect.xMin + spacing;
        rRect.xMin += halfSpacing;

        // Store old indent level and set it to 0, the PrefixLabel takes care of it.
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Draw properties.
        EditorGUI.LabelField(qLabelRect, "Q");
        EditorGUI.PropertyField(qRect, qAxis, GUIContent.none);
        EditorGUI.LabelField(rLabelRect, "R");
        EditorGUI.PropertyField(rRect, rAxis, GUIContent.none);

        if (EditorGUI.EndChangeCheck())
            property.serializedObject.ApplyModifiedProperties();

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }
}
