using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.AssetManagement;
using Lotl.Runtime.Generic;

namespace Lotl.Runtime
{
    [CreateAssetMenu(fileName = "Pool", menuName = "Lotl/Runtime/Pool")]
    public class Pool : ScriptableObject
    {
        #region Properties

        [SerializeField] private PoolMemberPrefabReference prefabReference;

        private HashSet<GameObject> activeItems = new();
        private HashSet<GameObject> inactiveItems = new();

        private Transform poolParent;
        
        public IReadOnlyCollection<GameObject> ActiveItems => activeItems;
        public IReadOnlyCollection<GameObject> InactiveItems => inactiveItems;

        #endregion

        /// <summary>
        /// Clears the pool, but DOES NOT DESTROY the objects it has created.
        /// </summary>
        public void Clear()
        {
            poolParent = null;

            activeItems.Clear();
            inactiveItems.Clear();
        }

        /// <summary>
        /// Clears the pool, deletes the parent of the objects it has created and creates a new parent.
        /// </summary>
        public void Initialize()
        {
            if (poolParent != null)
                Destroy(poolParent.gameObject);

            Clear();

            poolParent = new GameObject($"Pool [{name}] Object Parent").transform;
            poolParent.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        }

        public GameObject GetObject()
        {
            if (poolParent == null)
                Initialize();
            
            GameObject item = null;
            
            if(inactiveItems.Count != 0)
            {
                item = inactiveItems.FirstOrDefault();
                inactiveItems.Remove(item);
                activeItems.Add(item);
                return item;
            }

            GameObject prefab = prefabReference.GetPrefab();
            prefab.SetActive(false);
            item = Instantiate(prefab, poolParent);
            prefab.SetActive(true);

            if (!item.TryGetComponent<PoolMember>(out var poolMember))
            {
                Debug.LogError($"Pool [{name}] PrefabReference prefab is missing PoolMember component!");
                Destroy(item);
                return null;
            }

            poolMember.OnDeactivate += HandleItemDeactivation;
            poolMember.IsBeingDestroyed += HandleItemDestruction;

            activeItems.Add(item);
            
            return item;
        }

        private void HandleItemDeactivation(PoolMember poolMember)
        {
            activeItems.Remove(poolMember.gameObject);
            inactiveItems.Add(poolMember.gameObject);
        }

        private void HandleItemDestruction(NotifyOnDestroy sender)
        {
            if (sender is not PoolMember poolMember)
            {
                Debug.LogError($"Pool [{name}] received a destruction event with an incorrect sender!" +
                    $"Sender was: {sender}.");
                return;
            }

            activeItems.Remove(poolMember.gameObject);
            inactiveItems.Remove(poolMember.gameObject);

            poolMember.OnDeactivate -= HandleItemDeactivation;
            poolMember.IsBeingDestroyed -= HandleItemDestruction;
        }
    }
}
