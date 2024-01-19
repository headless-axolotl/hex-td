using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.Data.Towerset
{
    public class TowerIdentifier : MonoBehaviour
    {
        [SerializeField] private TowerIdentity identity;

        public TowerIdentity Identity => identity;
    }
}
