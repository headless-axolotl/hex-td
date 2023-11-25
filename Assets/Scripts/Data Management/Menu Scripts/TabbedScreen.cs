using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.Data.Menu
{
    public class TabbedScreen : MonoBehaviour
    {
        [SerializeField] protected MenuManager dataManager;

        public void Initialize(MenuManager dataManager)
        {
            this.dataManager = dataManager;
        }
    }
}
