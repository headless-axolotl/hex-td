using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.Data.Menu
{
    public class TabbedScreen : MonoBehaviour
    {
        [SerializeField] protected MenuManager menuManager;

        public void Initialize(MenuManager menuManager)
        {
            this.menuManager = menuManager;
        }
    }
}
