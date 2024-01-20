using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Lotl.Generic.Variables;
using Lotl.Data.Towerset;
using Lotl.Units;

namespace Lotl.Gameplay
{
    public class TowerInspector : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private TowerBuilder towerBuilder;
        [SerializeField] private HexReference selectedHex;
        [Header("UI")]
        [SerializeField] private GameObject towerInspector;
        [SerializeField] private TMP_Text towerName;
        [SerializeField] private TMP_Text towerDescription;
        [SerializeField] private Slider healthbar;

        private Unit selectedTower = null;

        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
                InspectHex();

            if(Input.GetKeyDown(KeyCode.E))
                UntrackTower(selectedTower);
        }

        private void InspectHex()
        {
            bool towerExists = towerBuilder.OccupiedPositions.ContainsKey(selectedHex);
            if (!towerExists)
            {
                UntrackTower(selectedTower);
                return;
            }

            Unit tower = towerBuilder.OccupiedPositions[selectedHex];
            
            if(!tower.TryGetComponent<TowerIdentifier>(out var identifier))
            {
                Debug.LogWarning($"Inspected unit at hex {selectedHex.Value} does" +
                    $"not have a TowerIdentifier Component!");
                return;
            }

            TowerIdentity identity = identifier.Identity;

            towerName.text = identity.TowerName;
            towerDescription.text = identity.TowerDescription;

            TrackTower(tower);
        }

        private void TrackTower(Unit tower)
        {
            selectedTower = tower;
            
            selectedTower.HealthChanged += UpdateHealthbar;
            selectedTower.Died += UntrackTower;
            
            UpdateHealthbar(selectedTower.Health);

            towerInspector.SetActive(true);
        }

        private void UntrackTower(Unit tower)
        {
            if (tower == null) return;

            tower.HealthChanged -= UpdateHealthbar;
            tower.Died -= UntrackTower;

            towerInspector.SetActive(false);
        }

        private void UpdateHealthbar(float amount)
        {
            float fraction = amount / selectedTower.MaxHealth;
            healthbar.value = fraction;
        }
    }
}