using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using Lotl.Hexgrid;
using Lotl.Runtime;
using Unity.VisualScripting;

[CustomEditor(typeof(HexTransform), true)]
public class HexTransform_Editor : Editor
{
    private HexTransform targetHexTransform;

    private void OnEnable()
    {
        targetHexTransform = target as HexTransform;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Set Transform From Hexgrid") && targetHexTransform != null)
        {
            targetHexTransform.HexPosition = targetHexTransform.HexPosition;
            EditorUtility.SetDirty(targetHexTransform);
        }

        if (GUILayout.Button("Set Hexgrid From Transform") && targetHexTransform != null)
        {
            targetHexTransform.WorldPosition = targetHexTransform.transform.position;
            EditorUtility.SetDirty(targetHexTransform);
        }
    }

}
