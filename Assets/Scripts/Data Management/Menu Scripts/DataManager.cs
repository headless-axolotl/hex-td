using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Data.Towerset;
using Lotl.Data.Runs;

namespace Lotl.Data.Menu
{
    public class DataManager : MonoBehaviour
    {
        #region Properties

        [Header("Data")]
        [SerializeField] private TowerTokenLibrary towerTokenLibrary;
        [SerializeField] private RunDataObject runData;

        [Header("Database")]
        [SerializeField] private RunTable runTableManager;
        // [SerializeField] private TableManager towersetTableManager;

        #endregion

        #region Methods

        #endregion
    }
}
