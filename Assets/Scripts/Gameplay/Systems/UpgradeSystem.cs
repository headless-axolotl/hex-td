using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Lotl.Generic.Variables;
using Lotl.Data.Towerset;
using Lotl.Units;
using Lotl.UI;
using System.Linq;
using Lotl.Data.Menu;
using Lotl.Hexgrid;
using Lotl.Generic.Events;

namespace Lotl.Gameplay
{
    public class UpgradeSystem : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private TowerBuilder towerBuilder;
        [SerializeField] private HexReference selectedHex;
        [SerializeField] private IntVariable resources;
        [Header("UI")]
        [SerializeField] private GameObject towerInspector;
        [SerializeField] private TMP_Text towerName;
        [SerializeField] private TMP_Text towerDescription;
        [SerializeField] private Slider healthbar;
        [SerializeField] private GameObject upgradeSubinspector;
        [SerializeField] private DataView upgradeOptionPicker;
        [SerializeField] private Button confirmUpgradeButton;
        [Header("Events")]
        [SerializeField] private GameEvent OnTrackTower;
        [SerializeField] private GameEvent OnUntrackTower;
        [SerializeField] private HexVariable trackedTowerPosition;

        private Unit selectedTower = null;
        private TowerUpgradeOption selectedUpgradeOption = null;

        private Hex SelectedTowerPosition
        {
            get
            {
                if(!selectedTower.TryGetComponent(out HexTransform hexTransform))
                    return Hex.Zero;
                return hexTransform.HexPosition;
            }
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

        private void Update()
        {
            HandleInput();
        }

        private void HandleInput()
        {
            if (Input.GetMouseButtonDown(1))
                InspectPosition(selectedHex);

            if (Input.GetKeyDown(KeyCode.Space))
                UntrackTower(selectedTower);

            bool tryUpgrade = Input.GetKeyDown(KeyCode.E)
                || Input.GetKeyDown(KeyCode.U);
            if (tryUpgrade) UpgradeTower();
        }

        private void InspectPosition(Hex position)
        {
            bool towerExists = towerBuilder.OccupiedPositions.ContainsKey(position);
            if (!towerExists)
            {
                UntrackTower(selectedTower);
                return;
            }

            Unit tower = towerBuilder.OccupiedPositions[position];

            UpdateUI(tower);
            TrackTower(tower, position);
        }

        private void TrackTower(Unit tower, Hex position)
        {
            if (selectedTower != null)
            {
                selectedTower.HealthChanged -= UpdateHealthbar;
                selectedTower.Died -= UntrackTower;
            }

            selectedTower = tower;

            selectedTower.HealthChanged += UpdateHealthbar;
            selectedTower.Died += UntrackTower;

            UpdateHealthbar(selectedTower.Health);

            towerInspector.SetActive(true);

            trackedTowerPosition.Value = position;
            OnTrackTower.Raise();
        }

        private void UntrackTower(Unit tower)
        {
            if (tower == null) return;

            tower.HealthChanged -= UpdateHealthbar;
            tower.Died -= UntrackTower;

            towerInspector.SetActive(false);

            OnUntrackTower.Raise();
        }

        private void UpdateUI(Unit tower)
        {
            if (!tower.TryGetComponent<TowerIdentifier>(out var identifier))
            {
                Debug.LogWarning($"Inspected unit at hex {selectedHex.Value} does" +
                    $"not have a TowerIdentifier Component!");
                return;
            }

            TowerIdentity identity = identifier.Identity;

            towerName.text = identity.TowerName;
            towerDescription.text = identity.TowerDescription;

            UpdateUpgradeOptionsUI(identifier);
        }

        private void UpdateUpgradeOptionsUI(TowerIdentifier identifier)
        {
            bool hasUpgrades = identifier.Upgrades.Count != 0;
            upgradeSubinspector.SetActive(hasUpgrades);

            if (!hasUpgrades) return;

            List<object> upgradeOptions = identifier.Upgrades.Cast<object>().ToList();
            upgradeOptionPicker.SetData(upgradeOptions, Conversions.ConvertTowerUpgradeOption);
        }

        public void UpgradeTower()
        {
            if (selectedTower == null)
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

            GameObject baseTower = selectedTower.gameObject;
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
                towerBuilder.SetPosition(baseTowerPosition, selectedTower);

                Debug.Log("Error upgrading tower!");
                return;
            }

            resources.Value -= selectedUpgradeOption.UpgradeCost;
            Destroy(baseTower);
            InspectPosition(baseTowerPosition);
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

        private void UpdateHealthbar(float amount)
        {
            float fraction = amount / selectedTower.MaxHealth;
            healthbar.value = fraction;
        }
    }
}