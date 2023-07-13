using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.AssetManagement;
using Lotl.Generic.Variables;

namespace Lotl.Gameplay
{
    [CreateAssetMenu(fileName = "Tower Token", menuName = "Lotl/Gameplay/Tower Token")]
    public class TowerToken : ScriptableObject
    {
        [SerializeField] private PrefabReference prefab;
        [SerializeField] private IntReference resourceCost;
        [SerializeField] private Sprite icon;

        public PrefabReference Prefab => prefab;
        public int ResourceCost => resourceCost;
        public Sprite Icon => icon;
    }
}
