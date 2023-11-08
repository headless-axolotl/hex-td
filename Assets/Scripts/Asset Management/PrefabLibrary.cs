using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Utility;

namespace Lotl.AssetManagement
{
    [CreateAssetMenu(fileName = "Library", menuName = "Lotl/Asset Management/Prefab/Library")]
    public class PrefabLibrary : ScriptableObject
    {
        [SerializeField] private List<PrefabBook> books = new();
        public IReadOnlyList<PrefabBook> Books => books;

#if UNITY_EDITOR

        private void OnValidate()
        {
            UpdateBooks();
        }

        private void UpdateBooks()
        {
            books.ResetDuplicates();
            books.ShiftNonNull();
            RecalculateBookIdentities();
        }

        private void RecalculateBookIdentities()
        {
            for(int i = 0; i < books.Count; i++)
            {
                if (books[i] != null)
                    books[i].Id = i;
            }
        }

#endif
    
    }
}