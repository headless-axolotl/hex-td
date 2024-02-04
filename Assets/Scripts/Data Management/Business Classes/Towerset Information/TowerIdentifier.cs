using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Generic.Variables;

namespace Lotl.Data.Towerset
{
    public class TowerIdentifier : MonoBehaviour
    {
        [SerializeField] private TowerIdentity identity;
        [SerializeField] private List<TowerUpgradeOption> upgrades;
        [SerializeField] private bool removable;

        public TowerIdentity Identity => identity;
        public IReadOnlyList<TowerUpgradeOption> Upgrades => upgrades;
        public bool Removable => removable;
    }

    [System.Serializable]
    public class TowerUpgradeOption
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private IntReference upgradeCost;

        public GameObject Prefab => prefab;

        public TowerIdentity Identity
        {
            get
            {
                if (!prefab.TryGetComponent(out TowerIdentifier identifier))
                    return null;

                return identifier.Identity;
            }
        }

        public int UpgradeCost => upgradeCost;
    }
}
