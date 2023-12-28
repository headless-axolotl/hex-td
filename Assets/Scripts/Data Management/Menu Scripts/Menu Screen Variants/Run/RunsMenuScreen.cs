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
using Lotl.Data.Runs;
using Lotl.Data.Towerset;

namespace Lotl.Data.Menu
{
    using Identity = RunContext.Identity;

    public class RunsMenuScreen : TabbedScreen
    {
        [Header("Data")]
        [SerializeField] private DatabaseManager databaseManager;
        [SerializeField] private UserCookie userCookie;
        [SerializeField] private RunDataObject crossSceneData;
        [SerializeField] private StringReference gameplayScene; 

        [Header("UI")]
        [SerializeField] private DataView currentRunsView;
        [SerializeField] private DataView selectedRunTowersetPreview;
        [SerializeField] private Button openSubmenuButton;
        [SerializeField] private Button playButton;
        [SerializeField] private Button deleteButton;
        [SerializeField] private TMP_Text feedbackText;

        [Header("Prompts")]
        [SerializeField] private Prompt prompt;
        [SerializeField] private StringReference deletePromptQuestion;
        [SerializeField] private StringReference deletePromptDescription;
        [SerializeField] private StringReference selectedRunStringFormat;

        [Header("Warning Messages")]
        [SerializeField] private Color warningColor;
        [SerializeField] private StringReference noSelectedRun;
        [SerializeField] private StringReference noValidTowersets;

        [Header("Error Messages")]
        [SerializeField] private Color errorColor;
        [SerializeField] private StringReference databaseError;

        private List<Identity> currentRuns;
        
        private Identity selectedRun;

        private void Awake()
        {
            if (databaseManager == null)
            {
                Debug.LogError("Missing database manager!");
                return;
            }
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

            selectedRunTowersetPreview.ClearEntries();
            currentRunsView.Deselect();
            SetButtonStates(toCreate: true);

            UpdateCurrentRunsView();
            UpdateOpenSubmenuButtonState();
        }

        #region Data View Event Methods

        private void OnEnable()
        {
            Activate();

            currentRunsView.OnSelect += OnRunSelected;
            currentRunsView.OnDeselect += OnRunDeselected;
        }

        private void OnDisable()
        {
            currentRunsView.OnSelect -= OnRunSelected;
            currentRunsView.OnDeselect -= OnRunDeselected;
        }

        private void OnRunSelected()
        {
            int index = currentRunsView.SelectedEntry.Index;
            selectedRun = (Identity)currentRunsView.Data[index];
            UpdateSelectedRunTowersetPreviewFromDatabase();

            SetButtonStates(toCreate: false);
        }

        private void OnRunDeselected()
        {
            selectedRun.name = string.Empty;
            selectedRunTowersetPreview.ClearEntries();
            SetButtonStates(toCreate: true);
        }

        private void SetButtonStates(bool toCreate = true)
        {
            openSubmenuButton.interactable = toCreate;

            playButton.interactable =
            deleteButton.interactable = !toCreate;
        }

        #endregion

        private void UpdateCurrentRunsView()
        {
            var allRuns = databaseManager.RunManager.TrackedRuns;

            currentRuns = allRuns
                .Where(identity => identity.userId == userCookie.UserId)
                .ToList();

            IEnumerable<object> currentRunsViewData = currentRuns.Cast<object>();

            currentRunsView.SetData(
                currentRunsViewData,
                Conversions.ConvertRunIdentity);
        }

        private void UpdateSelectedRunTowersetPreviewFromDatabase()
        {
            databaseManager.RunManager.GetRunData(selectedRun,
            onCompleted: (result, data) =>
            {
                if(!result.WasSuccessful)
                {
                    Debug.LogWarning(result.Message);
                    ErrorFeedback(databaseError);
                    return;
                }

                UpdateSelectedRunTowersetPreview(data.TowersetInfo);
            });
        }

        private void UpdateSelectedRunTowersetPreview(TowersetInfo info)
        {
            IEnumerable<object> selectedRunTowersetPreviewData = info.TowerTokens.Cast<object>();
            
            selectedRunTowersetPreview.SetData(
                selectedRunTowersetPreviewData,
                Conversions.ConvertTowerToken);
        }

        private void UpdateOpenSubmenuButtonState()
        {
            var validTowersets = databaseManager.TowersetManager
                .TrackedTowersets.Where(keyValuePair => keyValuePair.Value)
                .ToList();

            bool validTowersetExists = validTowersets.Count != 0;

            openSubmenuButton.interactable = validTowersetExists;

            if (validTowersetExists)
            {
                feedbackText.text = string.Empty;
            }
            else
            {
                WarnFeedback(noValidTowersets);
            }
        }

        public void PlaySelectedRun()
        {
            if (string.IsNullOrEmpty(selectedRun.name))
            {
                WarnFeedback(noSelectedRun);
                return;
            }

            databaseManager.RunManager.GetRunData(selectedRun,
            onCompleted: (result, data) =>
            {
                if (!result.WasSuccessful)
                {
                    Debug.LogWarning(result.Message);
                    ErrorFeedback(databaseError);
                    return;
                }

                crossSceneData.Data = data;
                databaseManager.SceneTransitionInitializer.StartSceneTransition(gameplayScene);
            });
        }

        public void PromptDeleteSelectedRun()
        {
            if (string.IsNullOrEmpty(selectedRun.name))
            {
                WarnFeedback(noSelectedRun);
                return;
            }

            string promptDescription =
                deletePromptDescription + "\n" +
                string.Format(selectedRunStringFormat, selectedRun.name);

            prompt.Activate(
                deletePromptQuestion,
                promptDescription,
                ConfirmDeleteSelectedRun,
                null);
        }

        public void ConfirmDeleteSelectedRun()
        {
            databaseManager.RunManager.Delete(selectedRun,
            onCompleted: (result) =>
            {
                if(!result.WasSuccessful)
                {
                    Debug.LogWarning(result.Message);
                    ErrorFeedback(databaseError);
                    return;
                }

                int selectedIndex = currentRunsView.SelectedEntry.Index;
                currentRunsView.RemoveEntry(selectedIndex);
            });
        }
    }
}
