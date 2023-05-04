using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.AssetManagement;
using System;

namespace Lotl.Runtime
{
    public class Pool : ScriptableObject
    {
        [SerializeField] private PoolMemberPrefabReference prefabReference;

        private HashSet<GameObject> activeItems = new();
        public IReadOnlyCollection<GameObject> ActiveItems => activeItems;

        private HashSet<GameObject> inactiveItems = new();
        public IReadOnlyCollection<GameObject> InactiveItems => inactiveItems;

        private Transform poolParent;

        public void Initialize(Transform poolParent)
        {
            ResetPool();
            SetPoolParent(poolParent);
        }

        public void SetPoolParent(Transform poolParent) 
            => this.poolParent = poolParent;

        public void ResetPool()
        {
            foreach(GameObject item in activeItems)
                Destroy(item);
            activeItems.Clear();

            foreach (GameObject item in inactiveItems)
                Destroy(item);
            inactiveItems.Clear();
        }

        public GameObject GetObject()
        {
            if(poolParent == null)
            {
                Debug.LogError($"Pool [{name}] is missing a parent Transform! A Pool must be initialized before usage!");
                return null;
            }

            GameObject item = null;
            
            if(inactiveItems.Count != 0)
            {
                item = inactiveItems.FirstOrDefault();
                inactiveItems.Remove(item);
                activeItems.Add(item);
                return item;
            }

            item = Instantiate(prefabReference.GetPrefab(), poolParent);
            
            if(!item.TryGetComponent<PoolMember>(out var poolMember))
            {
                Debug.LogError($"Pool [{name}] PrefabReference prefab is missing PoolMembe component!");
                Destroy(item);
                return null;
            }

            poolMember.OnDeactivate += HandleItemDeactivation;
            poolMember.IsBeingDestroyed += HandleItemDestruction;

            activeItems.Add(item);
            
            return item;
        }

        private void HandleItemDeactivation(object sender, EventArgs _)
        {
            if(sender is not PoolMember poolMember)
            {
                Debug.LogError($"Pool [{name}] received a deactivation event with an incorrect sender!");
                return;
            }

            activeItems.Remove(poolMember.gameObject);
            inactiveItems.Add(poolMember.gameObject);
        }

        private void HandleItemDestruction(object sender, EventArgs _)
        {
            if (sender is not PoolMember poolMember)
            {
                Debug.LogError($"Pool [{name}] received a destruction event with an incorrect sender!");
                return;
            }

            activeItems.Remove(poolMember.gameObject);
            inactiveItems.Remove(poolMember.gameObject);

            poolMember.OnDeactivate -= HandleItemDeactivation;
            poolMember.IsBeingDestroyed -= HandleItemDeactivation;
        }
    }
}
