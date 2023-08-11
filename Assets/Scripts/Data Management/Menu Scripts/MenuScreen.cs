using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.Data.Menu
{
    public class MenuScreen : MonoBehaviour
    {
        [SerializeField] protected DataManager dataManager;

        public void Initialize(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
    }
}
