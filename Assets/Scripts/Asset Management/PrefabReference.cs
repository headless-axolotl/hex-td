using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.AssetManagement
{
    [CreateAssetMenu(fileName = "Reference", menuName = "Asset Management/Prefab/References/Basic Reference")]
    public class PrefabReference : ScriptableObject
    {
        [SerializeField] private PrefabLibrary library;
        public PrefabLibrary Library => library;


        [SerializeField] private int bookIndex = -1;
        public int BookIndex => bookIndex;
        
        [SerializeField] private int prefabIndex = -1;
        public int PrefabIndex => prefabIndex;

        public PrefabBook GetBook()
        {
            if (library == null || BookIndex == -1) return null;

            return library.Books[BookIndex];
        }

        public GameObject GetPrefab()
        {
            PrefabBook book = GetBook();

            if (book == null || PrefabIndex == -1) return null;
            
            return book.Prefabs[prefabIndex];
        }

        public virtual bool IsValidPrefab() => true;

        private void Validate()
        {
            if (library == null ||
                bookIndex < -1 ||
                bookIndex >= library.Books.Count)
            {
                bookIndex = -1;
                prefabIndex = -1;
                return;
            }

            if (bookIndex == -1 ||
                library.Books[BookIndex] == null ||
                prefabIndex < -1 ||
                prefabIndex >= library.Books[bookIndex].Prefabs.Count)
            {
                prefabIndex = -1;
            }
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            Validate();
        }

#endif

    }
}
