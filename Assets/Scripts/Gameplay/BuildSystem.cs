using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.UI;
using Lotl.Data.Runs;
using Lotl.Data.Towerset;

namespace Lotl.Gameplay
{
    public class BuildSystem : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private RunDataObject crossSceneData;
        [Header("Runtime")]
        [SerializeField] private TowerBuilder towerBuilder;
        [SerializeField] private TowerToken selectedTower = null;
        [Header("UI")]
        [SerializeField] private DataView availableTowersView;
        
        public TowerToken SelectedTower => selectedTower;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            availableTowersView.SetData(
                crossSceneData.Data.TowersetInfo
                    .TowerTokens.Select(item => (object)item).ToList(),
                Data.Menu.Conversions.ConvertTowerToken);

            availableTowersView.OnSelect += OnTowerSelected;
        }

        private void OnTowerSelected(object _, EventArgs __)
        {
            int selectedIndex = availableTowersView.SelectedEntry.Index;
            selectedTower = crossSceneData.Data.TowersetInfo.TowerTokens[selectedIndex];
        }
    }
}
