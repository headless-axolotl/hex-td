using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Lotl.UI;
using Lotl.Data.Users;
using Lotl.Generic.Variables;

namespace Lotl.Data.Menu
{
    using Identity = TowersetContext.Identity;

    public class TowersetManagerScreen : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private DatabaseManager databaseManager;
        [SerializeField] private UserCookie userCookie;

        [Header("UI")]
        [SerializeField] private DataView towersetsView;
        [SerializeField] private Button createButton;
        [SerializeField] private Button editButton;
        [SerializeField] private Button deleteButton;
        [SerializeField] private TMP_Text feedbackText;

        [Header("Prompts")]
        [SerializeField] private Prompt prompt;
        [SerializeField] private StringReference deletePromptQuestion;
        [SerializeField] private StringReference deletePromptDescription;
        [SerializeField] private StringReference selectedTowersetStringFormat;

        [Header("Additional Screens")]
        [SerializeField] private TowersetEditorScreen towersetEditorScreen;

        [Header("Warning Messages")]
        [SerializeField] private Color warningColor;
        [SerializeField] private StringReference noSelectedTowerset;

        [Header("Error Messages")]
        [SerializeField] private Color errorColor;
        [SerializeField] private StringReference databaseError;
        
        private List<Identity> currentTowersets;

        private Identity selectedTowerset;

        private void Awake()
        {
            if (databaseManager == null)
            {
                Debug.LogError("Missing database manager!");
                return;
            }

            selectedTowerset.name = string.Empty;
        }

        private void WarnFeedback(string message)
        {
            feedbackText.color = warningColor;
            feedbackText.text = message;
        }

        private void ErrorFeedback(string message)
        {
            feedbackText.color = errorColor;
            feedbackText.text = message;
        }

        public void Activate()
        {
            feedbackText.text = string.Empty;

            UpdateTowersetsView();
        }

        private void UpdateTowersetsView()
        {
            var allTowersets = databaseManager.TowersetManager.TrackedTowersets.Keys;

            currentTowersets = allTowersets
                .Where(identity => identity.userId == userCookie.UserId)
                .ToList();

            IEnumerable<object> towersetViewData = currentTowersets
                .Cast<object>();

            towersetsView.SetData(
                towersetViewData,
                Conversions.ConvertTowersetIdentity);
        }

        #region Data View Event Methods

        private void OnEnable()
        {
            Activate();

            towersetsView.OnSelect += OnTowersetSelected;
            towersetsView.OnDeselect += OnTowersetDeselected;
        }

        private void OnDisable()
        {
            towersetsView.OnSelect -= OnTowersetSelected;
            towersetsView.OnDeselect -= OnTowersetDeselected;
        }

        private void OnTowersetSelected()
        {
            int selectedIndex = towersetsView.SelectedEntry.Index;
            selectedTowerset = (Identity)towersetsView.Data[selectedIndex];

            SetButtonStates(false);
        }

        private void OnTowersetDeselected()
        {
            selectedTowerset.name = string.Empty;
            SetButtonStates(true);
        }

        #endregion

        private void SetButtonStates(bool toCreate = true)
        {
            createButton.interactable = toCreate;

            editButton.interactable =
            deleteButton.interactable = !toCreate;
        }

        public void CreateTowetset()
        {
            towersetEditorScreen.Activate(onCompleted: (result) =>
            {
                if (!result.WasSuccessful)
                {
                    Debug.LogWarning(result.Message);
                    ErrorFeedback(databaseError);
                    return;
                }

                towersetEditorScreen.gameObject.SetActive(true);
            });
        }

        public void EditSelectedTowerset()
        {
            if (string.IsNullOrEmpty(selectedTowerset.name))
            {
                WarnFeedback(noSelectedTowerset);
                return;
            }

            towersetEditorScreen.Activate(onCompleted: (result) =>
            {
                if (!result.WasSuccessful)
                {
                    Debug.LogWarning(result.Message);
                    ErrorFeedback(databaseError);
                    return;
                }

                towersetEditorScreen.gameObject.SetActive(true);

            }, selectedTowerset.name);
        }

        public void PromptDeleteSelectedTowerset()
        {
            if (string.IsNullOrEmpty(selectedTowerset.name))
            {
                WarnFeedback(noSelectedTowerset);
                return;
            }

            string promptDescription =
                deletePromptDescription + "\n" +
                string.Format(selectedTowersetStringFormat, selectedTowerset.name);

            prompt.Activate(
                deletePromptQuestion,
                promptDescription,
                ConfirmDeleteSelectedTowerset,
                null);
        }

        public void ConfirmDeleteSelectedTowerset()
        {
            databaseManager.TowersetManager.Delete(selectedTowerset, onCompleted: (result) =>
            {
                if (!result.WasSuccessful)
                {
                    Debug.LogWarning(result.Message);
                    ErrorFeedback(databaseError);
                    return;
                }

                int selectedIndex = towersetsView.SelectedEntry.Index;
                towersetsView.RemoveEntry(selectedIndex);
            });
        }

    }
}
