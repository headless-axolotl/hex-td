using Lotl.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.UI
{
    public class DataViewSelectShortcuts : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private DataView dataView;
        [Header("Configuration")]
        [SerializeField] private List<Prefix> prefixes = new();
        [SerializeField] private List<KeyCode> shortcuts = new();

        private void Update()
        {
            if (PauseSystem.IsGamePaused) return;
            PerformCheck();
        }

        private void PerformCheck()
        {
            if (!PrefixesAreValid()) return;
            if (!CheckShortcuts(out int index)) return;
            dataView.Select(index);
        }

        private bool PrefixesAreValid()
        {
            bool isValid = true;
            for (int i = 0; i < prefixes.Count; i++)
            {
                if (!prefixes[i].IsValid())
                {
                    isValid = false;
                    break;
                }
            }
            return isValid;
        }

        private bool CheckShortcuts(out int index)
        {
            for(int i = 0; i < shortcuts.Count; i++)
            {
                if (Input.GetKeyDown(shortcuts[i]))
                {
                    index = i;
                    return true;
                }
            }
            index = 0;
            return false;
        }

        [System.Serializable]
        public struct Prefix
        {
            public bool shouldBePresent;
            public List<KeyCode> modifiers;

            public readonly bool IsValid()
            {
                bool modifierIsPressed = false;

                for (int i = 0; i < modifiers.Count; i++)
                {
                    modifierIsPressed = modifierIsPressed || Input.GetKey(modifiers[i]);
                }

                return shouldBePresent ? modifierIsPressed : !modifierIsPressed;
            }
        }
    }
}