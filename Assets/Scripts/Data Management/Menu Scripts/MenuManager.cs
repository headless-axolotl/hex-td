using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.Data.Menu
{
    public class MenuManager : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private List<GameObject> screens;
        [SerializeField] private int currentActiveScreen = -1;

        public void SetActiveScreen(int screenIndex)
        {
            if (screenIndex < 0 || screenIndex >= screens.Count) return;
            if (currentActiveScreen >= 0 && currentActiveScreen < screens.Count)
                screens[currentActiveScreen].SetActive(false);
            currentActiveScreen = screenIndex;
            screens[currentActiveScreen].SetActive(true);
        }
    }
}
