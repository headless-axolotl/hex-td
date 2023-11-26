using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.UI;
using Lotl.Data.Towerset;

namespace Lotl.Data.Menu
{
    using TowersetIdentity = TowersetContext.Identity;

    public static class Conversions
    {
        public static DataView.Entry ConvertTowerToken(object toConvert)
        {
            TowerToken towerToken = (TowerToken)toConvert;
            return new(towerToken.TowerName, string.Empty);
        }

        public static DataView.Entry ConvertTowersetIdentity(object toConvert)
        {
            TowersetIdentity towersetIdentity = (TowersetIdentity)toConvert;
            return new(towersetIdentity.name, string.Empty);
        }
    }
}
