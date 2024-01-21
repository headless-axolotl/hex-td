using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.UI;
using Lotl.Data.Towerset;

namespace Lotl.Data.Menu
{
    using TowersetIdentity = TowersetContext.Identity;
    using RunIdentity = RunContext.Identity;

    public static class Conversions
    {
        public static DataView.Entry ConvertTowerToken(object toConvert)
        {
            TowerToken towerToken = (TowerToken)toConvert;
            string entryDescription =
                $"[Resource cost: {towerToken.ResourceCost}#]\n" +
                towerToken.TowerDescription;
            return new(towerToken.TowerName, entryDescription);
        }

        public static DataView.Entry ConvertTowerTokenWtihShopCost(object toConvert)
        {
            TowerToken towerToken = (TowerToken)toConvert;
            string entryDescription =
                $"[Deed cost: {towerToken.ShopCost}€]\n" +
                $"[Resource cost: {towerToken.ResourceCost}#]";
            return new(towerToken.TowerName, entryDescription);
        }

        public static DataView.Entry ConvertTowersetIdentity(object toConvert)
        {
            TowersetIdentity towersetIdentity = (TowersetIdentity)toConvert;
            return new(towersetIdentity.name, string.Empty);
        }

        public static DataView.Entry ConvertRunIdentity(object toConvert)
        {
            RunIdentity runIdentity = (RunIdentity)toConvert;
            return new(runIdentity.name, string.Empty);
        }

        public static DataView.Entry ConvertTowerUpgradeOption(object toConvert)
        {
            TowerUpgradeOption upgradeOption = (TowerUpgradeOption)toConvert;
            TowerIdentity towerIdentity = upgradeOption.Identity;
            string entryDescription =
                $"[Upgrade cost: {upgradeOption.UpgradeCost}#]\n" + 
                $"[Description]: {towerIdentity.TowerDescription}";
            return new(towerIdentity.TowerName, entryDescription);
        }
    }
}
