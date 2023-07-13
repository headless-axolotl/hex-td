using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Hexgrid;
using Lotl.DataManagement;
using Lotl.AssetManagement;
using Lotl.Units;

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

            tower = Instantiate(prefab);
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

            tower = Instantiate(prefab);

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
            unit.Died += (_, __) => { occupiedPositions.Remove(position); };
            
            hexTransform.Position = position;

            return true;
        }
    }
}
