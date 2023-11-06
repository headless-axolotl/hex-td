using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Data.Towerset;

namespace Lotl.Data
{
    public class DatabaseManager : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private TowerTokenLibrary towerTokenLibrary;

        public TowerTokenLibrary TowerTokenLibrary => towerTokenLibrary;

        private void Awake()
        {
            
        }
    }
}