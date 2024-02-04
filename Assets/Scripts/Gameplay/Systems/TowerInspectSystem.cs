using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Lotl.Generic.Variables;
using Lotl.Data.Towerset;
using Lotl.Units;
using Lotl.Hexgrid;
using Lotl.Generic.Events;

namespace Lotl.Gameplay
{
    public class TowerInspectSystem : MonoBehaviour
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

        [Header("Events")]
        [SerializeField] private GameEvent OnTrackTower;
        [SerializeField] private GameEvent OnUntrackTower;
        [SerializeField] private HexVariable trackedTowerPosition;

        private Unit selectedTower = null;
        private TowerIdentifier selectedTowerIdentifier = null;

        public Unit SelectedTower => selectedTower;
        public TowerIdentifier SelectedTowerIdentifier => selectedTowerIdentifier;

        private void Update()
        {
            HandleInput();
        }

        private void HandleInput()
        {
            if (Input.GetMouseButtonDown(1))
                InspectPosition(selectedHex);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                UntrackTower(selectedTower);
                ClearSelected();
            }
        }

        public void InspectPosition(Hex position)
        {
            bool towerExists = towerBuilder.OccupiedPositions.ContainsKey(position);
            if (!towerExists)
            {
                UntrackTower(selectedTower);
                ClearSelected();
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

            selectedTowerIdentifier = identifier;
            TowerIdentity identity = identifier.Identity;

            towerName.text = identity.TowerName;
            towerDescription.text = identity.TowerDescription;
        }

        private void ClearSelected()
        {
            selectedTower = null;
            selectedTowerIdentifier = null;
        }

        private void UpdateHealthbar(float amount)
        {
            float fraction = amount / selectedTower.MaxHealth;
            healthbar.value = fraction;
        }
    }
}