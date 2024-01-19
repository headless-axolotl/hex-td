using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.AssetManagement;
using Lotl.Generic.Variables;

namespace Lotl.Data.Towerset
{
    [CreateAssetMenu(fileName = "Tower Token", menuName = "Lotl/Data/Towerset/Tower Token")]
    public class TowerToken : ScriptableObject
    {
        [SerializeField] private TowerIdentity identity;
        [SerializeField] private PrefabReference prefab;
        [SerializeField] private IntReference resourceCost;
        [SerializeField] private IntReference shopCost;

        [SerializeField] private int libraryIndex;

        public string TowerName => identity.TowerName;
        public string TowerDescription => identity.TowerDescription;
        public PrefabReference Prefab => prefab;
        public int ResourceCost => resourceCost;
        public int ShopCost => shopCost;
        public Sprite Icon => identity.Icon;

        public int IndexInLibrary { get => libraryIndex; set => libraryIndex = value; }
    }
}
