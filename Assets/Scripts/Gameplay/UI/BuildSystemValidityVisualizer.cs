using Lotl.Generic.Variables;
using Lotl.Hexgrid;
using Lotl.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.Gameplay
{
    public class BuildSystemValidityVisualizer : MonoBehaviour
    {
        [Header("Runtime")]
        [SerializeField] private BuildSystem buildSystem;
        [SerializeField] private TowerBuilder towerBuilder;
        [SerializeField] private HexReference selectedHex;
        [SerializeField] private IntReference resources;
        [SerializeField] private FloatReference hexSize;
        [Header("Elements")]
        [SerializeField] private GameObject validStateElement;
        [SerializeField] private GameObject invalidStateElement;

        private GameObject currentStateElement;
        
        private bool TowerIsSelected => buildSystem.SelectedTowerToken != null;

        private void Awake()
        {
            currentStateElement = validStateElement;
        }

        private void Update()
        {
            UpdateElements();
        }

        private void UpdateElements()
        {
            if (!TowerIsSelected)
            {
                DisableElements();
                return;
            }
            UpdateValidity();
            UpdatePosition();
        }

        private void DisableElements()
        {
            validStateElement.SetActive(false);
            invalidStateElement.SetActive(false);
        }

        private void UpdateValidity()
        {
            bool validity = true;
            validity = validity && Hex.Distance(Hex.Zero, selectedHex) <= buildSystem.MapSize;
            validity = validity && buildSystem.SelectedTowerToken.ResourceCost <= resources;
            validity = validity && towerBuilder.IsValidPosition(selectedHex);
            validStateElement.SetActive(validity);
            invalidStateElement.SetActive(!validity);

            currentStateElement = validity ? validStateElement : invalidStateElement;
        }

        private void UpdatePosition()
        {
            currentStateElement.transform.position = Hexgrid.Hex.HexToPixel(selectedHex, hexSize).xz();
        }
    }
}