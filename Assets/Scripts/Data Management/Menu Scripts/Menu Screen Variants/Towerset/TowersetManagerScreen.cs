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

    public class TowersetManagerScreen : TabbedScreen
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

        [Header("Additional Screens")]
        [SerializeField] private TowersetEditorScreen towersetEditorScreen;

        [Header("Warning Messages")]
        [SerializeField] private Color warningColor;
        [SerializeField] private StringReference noSelectedTowerset;

        [Header("Error Messages")]
        [SerializeField] private Color errorColor;
        [SerializeField] private StringReference databaseError;
        
        private List<Identity> currentTowersets;

        private string selectedTowerset;

        private void Awake()
        {
            if (databaseManager == null)
            {
                Debug.LogError("Missing database manager!");
            }

            selectedTowerset = string.Empty;
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

            List<object> castCurrentTowersets = currentTowersets
                .Cast<object>()
                .ToList();

            towersetsView.SetData(
                castCurrentTowersets,
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

        private void OnTowersetSelected(object _, EventArgs __)
        {
            selectedTowerset = towersetsView.SelectedEntry.EntryName;
            SetButtonStates(false);
        }

        private void OnTowersetDeselected(object _, EventArgs __)
        {
            selectedTowerset = string.Empty;
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
            if (string.IsNullOrEmpty(selectedTowerset))
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

            }, selectedTowerset);
        }

        public void DeleteSelectedTowerset()
        {
            if (string.IsNullOrEmpty(selectedTowerset))
            {
                WarnFeedback(noSelectedTowerset);
                return;
            }

            Identity identity = new(selectedTowerset, userCookie.UserId);

            databaseManager.TowersetManager.Delete(identity, onCompleted: (result) =>
            {
                if(!result.WasSuccessful)
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
