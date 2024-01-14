using Lotl.AssetManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PrefabBook))]
public class PrefabBook_Editor : Editor
{
    private PrefabBook prefabBook;

    private void OnEnable()
    {
        prefabBook = target as PrefabBook;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Update"))
        {
            prefabBook.UpdatePrefabs();
        }
    }
}