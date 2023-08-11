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
        [SerializeField] private List<RunInfoTemplate> runTemplates;
        [SerializeField] private RunDataObject crossSceneRunData;

        [Header("Database")]
        [SerializeField] private RunTable runTable;
        [SerializeField] private TowersetTable towersetTable;

        [Header("UI")]
        [SerializeField] private List<MenuScreen> screens;
        [SerializeField] private int currentActiveScreen = -1;

        #region Shared Data

        private Dictionary<string, TowersetInfo> cachedTowersetInfos;

        public Dictionary<string, TowersetInfo> CachedTowersetInfos => cachedTowersetInfos;

        #endregion

        public TowerTokenLibrary TowerTokenLibrary => towerTokenLibrary;
        public IReadOnlyList<RunInfoTemplate> RunTemplates => runTemplates;
        public RunDataObject CrossSceneRunData => crossSceneRunData;
        
        public RunTable RunTable => runTable;
        public TowersetTable TowersetTable => towersetTable;

        #endregion

        #region Methods

        private void Awake()
        {
            runTable.CreateTable();
            towersetTable.CreateTable();
            foreach(MenuScreen screen in screens)
            {
                screen.Initialize(this);
            }

            InitializeSharedData();
        }
        
        public void SetActiveScreen(int screenIndex)
        {
            if (screenIndex < 0 || screenIndex >= screens.Count) return;
            if (currentActiveScreen >= 0 && currentActiveScreen < screens.Count)
                screens[currentActiveScreen].gameObject.SetActive(false);
            currentActiveScreen = screenIndex;
            screens[currentActiveScreen].gameObject.SetActive(true);
        }

        #region Shared Data
        
        private void InitializeSharedData()
        {
            cachedTowersetInfos = new();
        }

        public TowersetInfo GetTowersetInfo(string id)
        {
            if (!CachedTowersetInfos.TryGetValue(id, out var towersetInfo))
            {
                towersetInfo = CachedTowersetInfos[id]
                    = TowersetInfo.Deserialize(
                    TowersetTable.ReadData(id),
                    TowerTokenLibrary);
            }

            return towersetInfo;
        }

        #endregion

        #endregion
    }
}
