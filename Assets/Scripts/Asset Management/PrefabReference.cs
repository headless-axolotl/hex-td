using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.AssetManagement
{
    [CreateAssetMenu(fileName = "Reference", menuName = "Asset Management/Prefab/References/Basic Reference")]
    public class PrefabReference : ScriptableObject
    {
        #region Properties

        [SerializeField] private PrefabLibrary library;
        [SerializeField] private int bookIndex = -1;
        [SerializeField] private int prefabIndex = -1;
        
        public PrefabLibrary Library => library;
        public int BookIndex => bookIndex;
        public int PrefabIndex => prefabIndex;

        #endregion

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

        protected virtual bool IsValidPrefab() => true;

        private void Validate()
        {
            if (library == null
                || bookIndex < -1
                || bookIndex >= library.Books.Count)
            {
                bookIndex = -1;
                prefabIndex = -1;
                return;
            }

            if (bookIndex == -1
                || library.Books[BookIndex] == null
                || prefabIndex < -1
                || prefabIndex >= library.Books[bookIndex].Prefabs.Count
                || !IsValidPrefab())
            {
                prefabIndex = -1;
            }
        }

        public static GameObject Evaluate(PrefabLibrary library, int bookIndex, int prefabIndex)
        {
            if (library == null) return null;
            if (bookIndex < 0 || bookIndex >= library.Books.Count) return null;
            PrefabBook book = library.Books[bookIndex];
            if (prefabIndex < 0 || prefabIndex >= book.Prefabs.Count) return null;
            return book.Prefabs[prefabIndex];
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            Validate();
        }

#endif

    }
}
