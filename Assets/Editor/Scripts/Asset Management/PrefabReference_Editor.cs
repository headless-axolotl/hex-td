using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

using Lotl.AssetManagement;

[CustomEditor(typeof(PrefabReference), true)]
public class PrefabReference_Editor : Editor
{
    SerializedProperty library;
    SerializedProperty bookIndex;
    SerializedProperty prefabIndex;

    protected void OnEnable()
    {
        library = serializedObject.FindProperty("library");
        bookIndex = serializedObject.FindProperty("bookIndex");
        prefabIndex = serializedObject.FindProperty("prefabIndex");
    }

    public override void OnInspectorGUI()
    {
        if(target is not PrefabReference info)
            return;
        
        serializedObject.Update();
        
        EditorGUILayout.PropertyField(library);

        bool serializedBookIndex = SerializeBookIndex(info, serializedObject.ApplyModifiedProperties());
        if (!serializedBookIndex) return;

        bool serializedPrefabIndex = SerializePrefabIndex(info, serializedObject.ApplyModifiedProperties());
        if (!serializedPrefabIndex) return;

        serializedObject.ApplyModifiedProperties();
    }

    protected virtual bool SerializeBookIndex(PrefabReference info, bool libraryChanged)
    {
        int popupIndex = bookIndex.intValue + 1;
        if (libraryChanged) popupIndex = 0;

        if (info.Library == null)
        {
            bookIndex.intValue = -1;
            prefabIndex.intValue = -1;
            serializedObject.ApplyModifiedProperties();
            return false;
        }

        List<string> bookNames = info.Library.Books
            .Select((book, index) => book != null ? book.name : $"<Null-{index}>")
            .ToList();
        bookNames.Insert(0, "<None>");

        bookIndex.intValue =
            EditorGUILayout.Popup(
                "Book",
                popupIndex,
                bookNames.ToArray())
            - 1;

        return true;
    }

    protected virtual bool SerializePrefabIndex(PrefabReference info, bool bookChanged)
    {
        int popupIndex = prefabIndex.intValue + 1;
        if (bookChanged) popupIndex = 0;

        if (bookIndex.intValue < 0 || info.Library.Books[bookIndex.intValue] == null)
        {
            prefabIndex.intValue = -1;
            serializedObject.ApplyModifiedProperties();
            return false;
        }
        PrefabBook infoBook = info.Library.Books[bookIndex.intValue];

        List<string> prefabNames = infoBook.Prefabs
            .Select((prefab, index) => prefab != null ? prefab.name : $"<Null-{index}>")
            .ToList();
        prefabNames.Insert(0, "<None>");

        prefabIndex.intValue =
            EditorGUILayout.Popup(
                "Prefab",
                popupIndex,
                prefabNames.ToArray())
            - 1;

        return true;
    }

}
