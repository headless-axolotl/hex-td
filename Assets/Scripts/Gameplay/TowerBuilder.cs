using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Hexgrid;
using Lotl.Units;
using Lotl.AssetManagement;
using Lotl.Data.Runs;

namespace Lotl.Gameplay
{
    public class TowerBuilder : MonoBehaviour
    {
        [SerializeField] private PrefabLibrary prefabLibrary;
        private HashSet<Hex> occupiedPositions = new();

        public bool IsValidPosition(Hex position)
        {
            return !occupiedPositions.Contains(position);
        }

        private GameObject InstantiateInactive(GameObject prefab)
        {
            prefab.SetActive(false);
            GameObject instance = Instantiate(prefab);
            prefab.SetActive(true);
            return instance;
        }

        public bool TryCreate(TowerInfo towerInfo, out GameObject tower)
        {
            tower = null;
            
            if(!IsValidPosition(towerInfo.Position))
            {
                Debug.LogError("Tried placing a tower from tower info in an occupied position!");
                return false;
            }

            GameObject prefab = PrefabReference.Evaluate(prefabLibrary, towerInfo.BookIndex, towerInfo.PrefabIndex);
            if (prefab == null)
            {
                Debug.LogError("Prefab evaluated from tower info was not valid!");
                return false;
            }

            tower = InstantiateInactive(prefab);

            return SetTowerInfo(tower, towerInfo.Position, towerInfo.CurrentHealth);
        }

        public bool TryCreate(PrefabReference prefabReference, Hex position, out GameObject tower)
        {
            tower = null;

            if (!IsValidPosition(position))
            {
                Debug.LogError("Tried placing a tower from prefab reference in an occupied position!");
                return false;
            }

            GameObject prefab = prefabReference.GetPrefab();
            if (prefab == null)
            {
                Debug.LogError("Given prefab reference is not valid!");
                return false;
            }

            tower = InstantiateInactive(prefab);

            return SetTowerInfo(tower, position);
        }

        public bool SetTowerInfo(GameObject instantiatedTower, Hex position, float currentHealth = -1f)
        {
            if (!instantiatedTower.TryGetComponent<Unit>(out var unit)
                || !instantiatedTower.TryGetComponent<HexTransform>(out var hexTransform))
            {
                Debug.LogError("Instantiated tower is missing needed Unit and/or HexTransform components!");
                return false;
            }

            if(currentHealth != -1f) unit.SetCurrentHealth(currentHealth);
            
            occupiedPositions.Add(position);
#warning CHANGE THIS:
            unit.Died += (_) => { occupiedPositions.Remove(position); };
            
            hexTransform.HexPosition = position;

            instantiatedTower.SetActive(true);

            return true;
        }
    }
}
