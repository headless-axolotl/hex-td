using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.Units
{
    [CreateAssetMenu(fileName = "TribeMask", menuName = "Lotl/Units/Unit Tribe Mask")]
    public class UnitTribeMask : ScriptableObject
    {
        [SerializeField] private List<UnitTribe> tribes;

        public bool Contains(UnitTribe tribe, bool invert = false)
        {
            if(tribe == null)
            {
                Debug.LogError($"Mask [{name}] .Contains was given a null tribe!");
                return false;
            }

            bool value = tribes.Contains(tribe);
            return invert ? !value : value;
        }
    }
}
