using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.Units
{
    using Generic.Variables;

    public class Unit : MonoBehaviour
    {
        [SerializeField] private float health;
        public float Health => health;

        [SerializeField] private UnitTribe tribe;
        public UnitTribe Tribe {
            get
            {
                if (tribe == null)
                    Debug.LogError($"Unit [{name}] does not have a tribe!");
                return tribe;
            }
        }
    }
}

