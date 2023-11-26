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
        [SerializeField] private string towerName;
        [SerializeField] private PrefabReference prefab;
        [SerializeField] private IntReference resourceCost;
        [SerializeField] private IntReference shopCost;
        [SerializeField] private Sprite icon;

        [SerializeField] private int libraryIndex;

        public string TowerName => towerName;
        public PrefabReference Prefab => prefab;
        public int ResourceCost => resourceCost;
        public Sprite Icon => icon;

        public int IndexInLibrary { get => libraryIndex; set => libraryIndex = value; }
    }
}
