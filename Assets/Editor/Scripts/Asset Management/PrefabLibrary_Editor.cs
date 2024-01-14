using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using Lotl.AssetManagement;

[CustomEditor(typeof(PrefabLibrary))]
public class PrefabLibrary_Editor : Editor
{
    private PrefabLibrary prefabLibrary;

    private void OnEnable()
    {
        prefabLibrary = target as PrefabLibrary;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Update"))
        {
            prefabLibrary.UpdateBooks();
        }
    }
}
