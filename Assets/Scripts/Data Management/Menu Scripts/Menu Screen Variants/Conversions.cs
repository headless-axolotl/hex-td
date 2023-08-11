using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.UI;
using Lotl.Data.Towerset;

namespace Lotl.Data.Menu
{
    public static class Conversions
    {
        public static DataView.Entry ConvertWithToString(object value)
        {
            return new(value.ToString(), string.Empty);
        }

        public static DataView.Entry ConvertTowersetTableEntry(object entry)
        {
            return new(((TowersetTable.Entry)entry).id, string.Empty);
        }

        public static DataView.Entry ConvertTowerToken(object towerToken)
        {
            return new(((TowerToken)towerToken).TowerName, string.Empty);
        }
    }
}
