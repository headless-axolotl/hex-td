using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.Data.Towerset
{
    [CreateAssetMenu(fileName = "TowerIdentity", menuName = "Lotl/Data/Towerset/Tower Identity")]
    public class TowerIdentity : ScriptableObject
    {
        [SerializeField] private string towerName;
        [TextArea]
        [SerializeField] private string towerDescription;
        [SerializeField] private Sprite icon;

        public string TowerName => towerName;
        public string TowerDescription => towerDescription;
        public Sprite Icon => icon;
    }
}

