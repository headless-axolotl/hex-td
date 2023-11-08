using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Utility;

namespace Lotl.Data.Towerset
{
    [CreateAssetMenu(fileName = "TowerTokenLibrary", menuName = "Lotl/Data/Towerset/Tower Token Library")]
    public class TowerTokenLibrary : ScriptableObject
    {
        [SerializeField] private List<TowerToken> towerTokens = new();

        public IReadOnlyList<TowerToken> TowerTokens => towerTokens;

        public TowerToken GetToken(int index)
        {
            if(index < 0 || index >= towerTokens.Count) return null;
            return towerTokens[index];
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            UpdateTowerTokens();
        }

        private void UpdateTowerTokens()
        {
            towerTokens.ResetDuplicates();
            towerTokens.ShiftNonNull();
            UpdateTowerTokenIndeces();
        }

        private void UpdateTowerTokenIndeces()
        {
            for(int i = 0; i < towerTokens.Count; i++)
            {
                if (towerTokens[i] == null) break;
                towerTokens[i].IndexInLibrary = i;
            }
        }

#endif
    }
}

