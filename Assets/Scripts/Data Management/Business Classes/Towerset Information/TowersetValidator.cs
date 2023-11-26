using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.Data.Towerset
{
    public static class TowersetValidator
    {
        public static bool Validity(this TowersetInfo towersetInfo)
        {
            int towerCount = towersetInfo.TowerTokens.Count;
            bool validity = MinTowersetTowerCount <= towerCount
                && towerCount <= MaxTowersetTowerCount;
            return validity;
        }

        private const int MinTowersetTowerCount = 3;
        private const int MaxTowersetTowerCount = 5;
    }
}