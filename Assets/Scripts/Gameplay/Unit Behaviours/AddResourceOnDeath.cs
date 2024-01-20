using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Units;
using Lotl.Generic.Variables;

namespace Lotl.Gameplay.UnitBehaviours
{
    [RequireComponent(typeof(Unit))]
    public class AddResourceOnDeath : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private IntReference reward;
        [SerializeField] private FloatReference modifier;
        [SerializeField] private IntVariable resources;

        private Unit unit;

        private void Awake()
        {
            unit = GetComponent<Unit>();
            unit.Died += AddResources;
        }

        void AddResources(Unit unit)
        {
            int resourcesToAdd = (int)((float)reward * modifier);
            resources.Value += resourcesToAdd;
            unit.Died -= AddResources;
        }
    }
}
