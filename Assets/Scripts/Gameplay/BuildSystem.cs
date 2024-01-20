using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.UI;
using Lotl.Data.Runs;
using Lotl.Data.Towerset;
using Lotl.Generic.Variables;
using UnityEngine.EventSystems;

namespace Lotl.Gameplay
{
    public class BuildSystem : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private RunDataObject crossSceneData;
        [Header("Runtime")]
        [SerializeField] private TowerBuilder towerBuilder;
        [SerializeField] private TowerToken selectedTowerToken = null;
        [SerializeField] private HexReference selectedHex;
        [SerializeField] private IntVariable resources;
        [Header("UI")]
        [SerializeField] private DataView availableTowersView;
        
        public TowerToken SelectedTowerToken => selectedTowerToken;

        private void Awake()
        {
            Initialize();
        }

        private void Update()
        {
            HandleInput();
        }

        private void Initialize()
        {
            IEnumerable<object> availableTowersViewData = crossSceneData.Data
                .TowersetInfo.TowerTokens.Cast<object>();

            availableTowersView.SetData(
                availableTowersViewData,
                Data.Menu.Conversions.ConvertTowerToken);

            availableTowersView.OnSelect += OnTowerTokenSelected;
        }

        private void OnTowerTokenSelected()
        {
            int selectedIndex = availableTowersView.SelectedEntry.Index;
            selectedTowerToken = crossSceneData.Data.TowersetInfo.TowerTokens[selectedIndex];
        }

        private void HandleInput()
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                AttemptToPlaceTower();
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                DeselectTowerToken();
            }
        }

        private void AttemptToPlaceTower()
        {
            if (!towerBuilder.IsValidPosition(selectedHex))
            {
                Debug.Log("Invalid position to build a tower.");
                DeselectTowerToken();
                return;
            }
            
            if (selectedTowerToken == null)
            {
                Debug.Log("There isn't a selected tower to build.");
                return;
            }

            if (selectedTowerToken.ResourceCost > resources.Value)
            {
                Debug.Log($"Not enough resources to build tower {selectedTowerToken.TowerName}.");
                return;
            }
            
            if(!towerBuilder.TryCreate(selectedTowerToken.Prefab, selectedHex, out var _))
            {
                Debug.LogWarning($"Could not build tower {selectedTowerToken.TowerName} in location {selectedHex.Value}!");
                return;
            }

            resources.Value -= selectedTowerToken.ResourceCost;

            bool shouldDeselect = !(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
            if(shouldDeselect) DeselectTowerToken();
        }

        public void DeselectTowerToken()
        {
            availableTowersView.Deselect();
            selectedTowerToken = null;
        }
    }
}
