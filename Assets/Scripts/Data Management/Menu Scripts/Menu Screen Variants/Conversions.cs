using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.UI;
using Lotl.Data.Towerset;

namespace Lotl.Data.Menu
{
    public static class Conversions
    {
        public static DataView.Entry ConvertTowerToken(object towerToken)
        {
            return new(((TowerToken)towerToken).TowerName, string.Empty);
        }
    }
}
