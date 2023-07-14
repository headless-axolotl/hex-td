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
        [SerializeField] private int resources = 0;
        [SerializeField] private List<TowerInfo> towersData;

        public int Resources { get => resources; set => resources = value; }
        public List<TowerInfo> TowersData => towersData;

        public RunInfo()
        {
            towersData = new();
        }

        public void ExtractTowerInfo(TowerRuntimeSet towerRuntimeSet)
        {
            towersData.Clear();
            foreach (Tower tower in towerRuntimeSet.Items)
            {
                if (TowerInfo.TryExtractInfo(tower.gameObject, out var towerInfo))
                    towersData.Add(towerInfo);
            }
        }
    }
}
