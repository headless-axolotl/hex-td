using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Hexgrid;
using Lotl.Units;
using Lotl.Units.Attackers;
using Lotl.AssetManagement;

namespace Lotl.Data.Runs
{
    [System.Serializable]
    public class TowerInfo
    {
        [SerializeField] private int bookIndex;
        [SerializeField] private int prefabIndex;
        [SerializeField] private float currentHealth;
        [SerializeField] private Hex position;

        public int BookIndex { get => bookIndex; set => bookIndex = value; }
        public int PrefabIndex { get => prefabIndex; set => prefabIndex = value; }
        public float CurrentHealth { get => currentHealth; set => currentHealth = value; }
        public Hex Position { get => position; set => position = value; }

        public static bool TryExtractInfo(GameObject tower, out TowerInfo info)
        {
            info = null;

            if(!tower.TryGetComponent<PrefabIdentity>(out var prefabIdentity)
                || !tower.TryGetComponent<Unit>(out var unit)
                || !tower.TryGetComponent<HexTransform>(out var hexTransform))
                return false;

            info = new()
            {
                BookIndex = prefabIdentity.BookId,
                PrefabIndex = prefabIdentity.Id,
                CurrentHealth = unit.Health,
                Position = hexTransform.HexPosition,
            };

            return true;
        }

        public override string ToString()
        {
            return $"TowerInfo{{Book: {BookIndex}, Prefab: {PrefabIndex}, CurrentHealth: {CurrentHealth}, Position: {Position}}}";
        }
    }
}
