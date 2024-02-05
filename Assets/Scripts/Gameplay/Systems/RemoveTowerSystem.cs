using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Lotl.UI;
using Lotl.Generic.Variables;

namespace Lotl.Gameplay
{
    public class RemoveTowerSystem : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private TowerBuilder towerBuilder;
        [SerializeField] private TowerInspectSystem towerInspector;
        [Header("UI")]
        [SerializeField] private Button removeTowerButton;
        [Header("Prompt")]
        [SerializeField] private Prompt prompt;
        [SerializeField] private StringReference promptQuestion;
        [SerializeField] private StringReference promptDescription;

        private void Update()
        {
            HandleInput();
        }

        private void HandleInput()
        {
            if (PauseSystem.IsGamePaused) return;

            if (Input.GetKeyDown(KeyCode.R))
                StartRemoveTower();
        }

        public void StartRemoveTower()
        {
            bool shouldPrompt = !(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
            
            if (shouldPrompt)
            {
                prompt.Activate(
                    promptQuestion,
                    promptDescription,
                    confirm: RemoveTower,
                    cancel: null);
            }
            else
            {
                RemoveTower();
            }
        }

        private void RemoveTower()
        {
            bool towerIsRemovable = towerInspector.SelectedTowerIdentifier.Removable;
            if (!towerIsRemovable) return;

            towerInspector.SelectedTower.SetCurrentHealth(0f);
        }

        public void UpdateRemoveButton()
        {
            if (towerInspector.SelectedTowerIdentifier == null) return;

            bool towerIsRemovable = towerInspector.SelectedTowerIdentifier.Removable;
            removeTowerButton.gameObject.SetActive(towerIsRemovable);
        }
    }
}