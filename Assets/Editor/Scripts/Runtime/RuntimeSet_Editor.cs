using UnityEngine;
using UnityEditor;

using Lotl.Runtime;

[CustomEditor(typeof(RuntimeSetBase), true)]
public class RuntimeSet_Editor : Editor
{
    private RuntimeSetBase targetPool;

    private void OnEnable()
    {
        targetPool = target as RuntimeSetBase;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Clear") && targetPool != null)
        {
            targetPool.Clear();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
