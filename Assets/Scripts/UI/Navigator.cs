using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Lotl.UI
{
    public class Navigator : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private KeyCode navigateKey;
        [SerializeField] private KeyCode reverseDirectionKey;
        [Header("UI")]
        [SerializeField] private List<Selectable> selectables;

        void Update()
        {
            UpdateSelection();
        }

        private void UpdateSelection()
        {
            if (!Input.GetKeyDown(navigateKey)) return; 
            bool reverse = Input.GetKey(reverseDirectionKey);

            int index = IndexOfCurrentSelected();
            if (index == -1) return;

            SelectNext(index, reverse);
        }

        private int IndexOfCurrentSelected()
        {
            GameObject currentSelected = EventSystem.current.currentSelectedGameObject;
            for (int i = 0; i < selectables.Count; i++)
            {
                bool selectableMatches = selectables[i].gameObject == currentSelected;
                if (selectableMatches) return i;
            }
            return -1;
        }

        private void SelectNext(int index, bool reverse = false)
        {
            int direction = reverse ? -1 : 1;
            index = (index + selectables.Count + direction) % selectables.Count;
            selectables[index].Select();
        }
    }
}