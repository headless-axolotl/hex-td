using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using Lotl.Runtime;

[CustomEditor(typeof(Pool))]
public class Pool_Editor : Editor
{
    private Pool targetPool;

    private void OnEnable()
    {
        targetPool = target as Pool;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Clear") && targetPool != null)
        {
            targetPool.Clear();
            serializedObject.ApplyModifiedProperties();
        }

        if (GUILayout.Button("State"))
        {
            Debug.Log($"Inspected pool has {targetPool.ActiveItems.Count} active items and " +
                $"{targetPool.InactiveItems.Count} inactive items.");
        }
    }
}
