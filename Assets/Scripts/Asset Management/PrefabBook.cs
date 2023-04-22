using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

using Lotl.Extensions;

namespace Lotl.AssetManagement
{
    [CreateAssetMenu(fileName = "Book", menuName = "Asset Management/Prefab/Book")]
    public class PrefabBook : ScriptableObject
    {
        [SerializeField] private int id;
        public int Id { get => id; set => id = value; }

        [SerializeField] private List<GameObject> prefabs = new();
        public IReadOnlyList<GameObject> Prefabs => prefabs;

#if UNITY_EDITOR

        private void OnValidate()
        {
            UpdatePrefabs();
        }

        private void UpdatePrefabs()
        {
            RemoveInvalidPrefabs();
            prefabs.ResetDuplicates();
            prefabs.ShiftNonNull();
            UpdatePrefabIdentities();
        }

        private void RemoveInvalidPrefabs()
        {
            List<GameObject> prefabsToRemove = new();
            for (int i = 0; i < prefabs.Count; i++)
            {
                if (prefabs[i] == null)
                    continue;
                if (prefabs[i].GetComponent<PrefabIdentity>() == null)
                    prefabsToRemove.Add(prefabs[i]);
            }

            foreach (GameObject prefab in prefabsToRemove)
            {
                prefabs.Remove(prefab);
            }
        }

        private void UpdatePrefabIdentities()
        {
            for (int i = 0; i < prefabs.Count; i++)
            {
                if (prefabs[i] != null)
                {
                    PrefabIdentity identity = prefabs[i].GetComponent<PrefabIdentity>();
                    identity.Id = i;
                    identity.PrefabBook = this;
                }
            }
        }

#endif
    
    }
}
