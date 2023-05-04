using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.AssetManagement
{
    public class BehaviourConstrainedPrefabReference<T> : PrefabReference
        where T : MonoBehaviour
    {
        protected override bool IsValidPrefab()
        {
            GameObject prefab = GetPrefab();
            if (prefab == null) return true;
            if (prefab.GetComponent<T>() != null) return true;
            return false;
        }
    }
}