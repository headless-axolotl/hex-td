using UnityEngine;
using UnityEditor;

using Lotl.Runtime;

[CustomEditor(typeof(RuntimeSetBase), true)]
public class RuntimeSet_Editor : Editor
{
    private RuntimeSetBase targetRuntimeSet;

    private void OnEnable()
    {
        targetRuntimeSet = target as RuntimeSetBase;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Clear") && targetRuntimeSet != null)
        {
            targetRuntimeSet.Clear();
            serializedObject.ApplyModifiedProperties();
        }

        if (GUILayout.Button("State") && targetRuntimeSet != null)
        {
            Debug.Log($"Inspected runtime set has {targetRuntimeSet.Count()} items.");
        }
    }
}
