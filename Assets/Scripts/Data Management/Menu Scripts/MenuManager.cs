using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.Data.Menu
{
    public class MenuManager : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private List<TabbedScreen> screens;
        [SerializeField] private int currentActiveScreen = -1;

        private void Awake()
        {
            
        }

        public void SetActiveScreen(int screenIndex)
        {
            if (screenIndex < 0 || screenIndex >= screens.Count) return;
            if (currentActiveScreen >= 0 && currentActiveScreen < screens.Count)
                screens[currentActiveScreen].gameObject.SetActive(false);
            currentActiveScreen = screenIndex;
            screens[currentActiveScreen].gameObject.SetActive(true);
        }
    }
}
