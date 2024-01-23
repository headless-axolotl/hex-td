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
            if (!PrefixesAreValid()) return;
            CheckShortcuts();
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

        private void CheckShortcuts()
        {
            for(int i = 0; i < shortcuts.Count; i++)
            {
                if (Input.GetKeyDown(shortcuts[i]))
                {
                    dataView.Select(i);
                    return;
                }
            }
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