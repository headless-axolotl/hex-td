using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lotl.UI
{
    public class SelectableColorManager : MonoBehaviour
    {
        [Header("Objects")]
        [SerializeField] private List<Image> selectableGraphics;
        private int selectedButton = -1;
        [Header("Settings")]
        [SerializeField] private Color highlighted;
        [SerializeField] private Color unhighlighted;

        private void Awake()
        {
            foreach(Image image in selectableGraphics)
            {
                image.color = unhighlighted;
            }
        }

        public void Select(int index)
        {
            if(selectedButton >= 0 && selectedButton <= selectableGraphics.Count)
                selectableGraphics[selectedButton].color = unhighlighted;
            if (index >= 0 && index <= selectableGraphics.Count)
                selectableGraphics[index].color = highlighted;
            selectedButton = index;
        }
    }
}

