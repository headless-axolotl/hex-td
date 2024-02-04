using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Lotl.UI;
using Lotl.Data.Towerset;
using Lotl.Data.Menu;
using Lotl.Hexgrid;
using Lotl.Generic.Variables;

namespace Lotl.Gameplay
{
    public class UpgradeSystem : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private TowerBuilder towerBuilder;
        [SerializeField] private TowerInspectSystem towerInspector;
        [SerializeField] private IntVariable resources;
        [Header("UI")]
        [SerializeField] private GameObject upgradeSubinspector;
        [SerializeField] private DataView upgradeOptionPicker;
        [SerializeField] private Button confirmUpgradeButton;

        private TowerUpgradeOption selectedUpgradeOption = null;

        private Hex SelectedTowerPosition
        {
            get
            {
                if (!towerInspector.SelectedTower.TryGetComponent(out HexTransform hexTransform))
                    return Hex.Zero;
                return hexTransform.HexPosition;
            }
        }

        private void Update()
        {
            HandleInput();
        }

        private void HandleInput()
        {
            bool tryUpgrade = Input.GetKeyDown(KeyCode.E)
                || Input.GetKeyDown(KeyCode.U);
            if (tryUpgrade) UpgradeTower();
        }

        private void OnEnable()
        {
            upgradeOptionPicker.OnSelect += OnUpgradeOptionSelected;
            upgradeOptionPicker.OnDeselect += OnUpgradeOptionDeselected;
        }

        private void OnDisable()
        {
            upgradeOptionPicker.OnSelect -= OnUpgradeOptionSelected;
            upgradeOptionPicker.OnDeselect -= OnUpgradeOptionDeselected;
        }

        public void UpdateUpgradeOptionsUI()
        {
            if (towerInspector.SelectedTowerIdentifier == null) return;

            bool hasUpgrades = towerInspector.SelectedTowerIdentifier.Upgrades.Count != 0;
            upgradeSubinspector.SetActive(hasUpgrades);

            if (!hasUpgrades) return;

            List<object> upgradeOptions = towerInspector.SelectedTowerIdentifier.Upgrades.Cast<object>().ToList();
            upgradeOptionPicker.SetData(upgradeOptions, Conversions.ConvertTowerUpgradeOption);
        }

        public void UpgradeTower()
        {
            if (towerInspector.SelectedTower == null)
            {
                Debug.Log("There is no selected tower to upgrade!");
                return;
            }

            if (selectedUpgradeOption == null)
            {
                Debug.Log("There is no selected upgrade option!");
                return;
            }

            bool canUpgrade = selectedUpgradeOption.UpgradeCost <= resources.Value;

            if (!canUpgrade)
            {
                Debug.Log("There are not enough resources to upgrade this tower!");
                return;
            }

            GameObject baseTower = towerInspector.SelectedTower.gameObject;
            Hex baseTowerPosition = SelectedTowerPosition;

            baseTower.SetActive(false);
            towerBuilder.FreePosition(baseTowerPosition);

            bool tryCreateUpgradedTower = towerBuilder.TryCreate(
                selectedUpgradeOption.Prefab,
                baseTowerPosition,
                out var _);

            if (!tryCreateUpgradedTower)
            {
                baseTower.SetActive(true);
                towerBuilder.SetPosition(baseTowerPosition, towerInspector.SelectedTower);

                Debug.Log("Error upgrading tower!");
                return;
            }

            resources.Value -= selectedUpgradeOption.UpgradeCost;
            Destroy(baseTower);
            towerInspector.InspectPosition(baseTowerPosition);
        }

        private void OnUpgradeOptionSelected()
        {
            int selectedIndex = upgradeOptionPicker.SelectedEntry.Index;
            selectedUpgradeOption = upgradeOptionPicker.Data[selectedIndex] as TowerUpgradeOption;
            confirmUpgradeButton.interactable = true;
        }

        private void OnUpgradeOptionDeselected()
        {
            selectedUpgradeOption = null;
            confirmUpgradeButton.interactable = false;
        }
    }
}