using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Runtime;
using Lotl.Units.Towers;

namespace Lotl.DataManagement
{
    [System.Serializable]
    public class RunInfo
    {
        [SerializeField] private int resourceAmount = 0;
        [SerializeField] private List<TowerInfo> towersData;

        public int ResourceAmount { get => resourceAmount; set => resourceAmount = value; }
        public List<TowerInfo> TowersData => towersData;

        public RunInfo()
        {
            towersData = new();
        }

        public void ExtractTowerInfo(TowerRuntimeSet towerRuntimeSet)
        {
            foreach (Tower tower in towerRuntimeSet.Items)
            {
                if (TowerInfo.TryExtractInfo(tower.gameObject, out var towerInfo))
                    towersData.Add(towerInfo);
            }
        }
    }
}
