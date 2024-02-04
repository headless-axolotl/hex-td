using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;

using Lotl.UI;
using Lotl.Data.Users;
using Lotl.Generic.Variables;
using Lotl.Data.Towerset;

namespace Lotl.Data.Menu
{
    using Result = Utility.Async.AsyncTaskResult;

    public class TowersetEditorScreen : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private DatabaseManager databaseManager;
        [SerializeField] private UserCookie userCookie;

        [Header("UI")]
        [SerializeField] private TMP_Text screenTitle;
        [Space]
        [SerializeField] private TMP_InputField towersetNameInput;
        [SerializeField] private Button renameButton;
        [SerializeField] private Button confirmRenamingButton;
        [Space]
        [SerializeField] private DataView availableTowersView;
        [SerializeField] private DataView towersInCurrentTowersetView;
        [SerializeField] private Button saveButton;
        [Space]
        [SerializeField] private Button addTowerButton;
        [SerializeField] private Button removeTowerButton;
        [Space]
        [SerializeField] private TMP_Text feedbackText;

        [Header("Title Variations")]
        [SerializeField] private StringReference titleForCreating;
        [SerializeField] private StringReference titleForEditing;

        [Header("Warning Messages")]
        [SerializeField] private Color warningColor;
        [SerializeField] private StringReference towersetAlreadyExists;
        [SerializeField] private StringReference towersetNameCannotBeEmpty;

        [Header("Error Messages")]
        [SerializeField] private Color errorColor;
        [SerializeField] private StringReference databaseError;

        private HashSet<TowerToken> towersInCurrentTowerset = new();
        private string oldTowersetName;

        private void Awake()
        {
            if (databaseManager == null)
            {
                Debug.LogError("Missing database manager!");
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

        #region Activation

        public void Activate(
            Action<Result> onCompleted,
            string selectedTowersetName = null)
        {
            feedbackText.text = string.Empty;
            towersetNameInput.text = string.Empty;

            UpdateAvailableTowersViewFromDatabase(onCompleted: (result) =>
            {
                if(!result.WasSuccessful)
                {
                    Debug.LogWarning(result.Message);
                    onCompleted?.Invoke(result);
                    return;
                }

                ContinueActivation(onCompleted, selectedTowersetName);
            });
        }

        private void ContinueActivation(
            Action<Result> inheritedOnCompleted,
            string selectedTowersetName = null)
        {
            towersInCurrentTowerset.Clear();

            SetScreenMode(selectedTowersetName);

            bool isInCreateMode = string.IsNullOrEmpty(selectedTowersetName);
            if (isInCreateMode)
            {
                towersInCurrentTowersetView.ClearEntries();
                inheritedOnCompleted?.Invoke(Result.OK);
                return;
            }

            UpdateTowersInCurrentTowersetViewFromDatabase(selectedTowersetName, inheritedOnCompleted);
        }

        private void UpdateAvailableTowersViewFromDatabase(
            Action<Result> onCompleted)
        {
            databaseManager.UserManager.GetUserData(userCookie.UserId,
            onCompleted: (result, data) =>
            {
                if(!result.WasSuccessful)
                {
                    Debug.LogWarning(result.Message);
                    onCompleted?.Invoke(result);
                    return;
                }
                
                var unlockedTowerTokens = data.UnlockedTowers.TowerTokens;
                UpdateAvailableTowersView(unlockedTowerTokens);

                onCompleted?.Invoke(Result.OK);
            });
        }

        private void UpdateTowersInCurrentTowersetViewFromDatabase(
            string currentTowerset,
            Action<Result> onCompleted)
        {
            TowersetContext.Identity towersetIdentity = new(currentTowerset, userCookie.UserId);

            databaseManager.TowersetManager.GetTowersetInfo(towersetIdentity,
            onCompleted: (result, info) =>
            {
                if (!result.WasSuccessful)
                {
                    Debug.LogWarning(result.Message);
                    onCompleted?.Invoke(result);
                    return;
                }

                towersInCurrentTowerset.AddRange(info.TowerTokens);
                
                UpdateTowersInCurrentTowersetView(towersInCurrentTowerset);

                onCompleted?.Invoke(Result.OK);
            });
        }

        #endregion

        #region Utility

        private void SetScreenMode(string towersetName = null)
        {
            bool inCreateMode = string.IsNullOrEmpty(towersetName);

            saveButton.interactable = true;

            if (inCreateMode)
            {
                screenTitle.text = titleForCreating;

                towersetNameInput.interactable = true;
                renameButton.interactable = false;

                return;
            }

            screenTitle.text = titleForEditing + towersetName;

            towersetNameInput.text = towersetName;
            towersetNameInput.interactable = false;
            renameButton.interactable = true;
        }

        private void UpdateAvailableTowersView(
            IEnumerable<TowerToken> data)
        {
            IEnumerable<object> availableTowersViewData = data
                .OrderBy(token => token.IndexInLibrary)
                .Cast<object>();

            availableTowersView.SetData(
                availableTowersViewData,
                Conversions.ConvertTowerToken);
        }

        private void UpdateTowersInCurrentTowersetView(
            IEnumerable<TowerToken> data)
        {
            IEnumerable<object> towersInCurrentTowersetViewData = data
                .OrderBy(token => token.IndexInLibrary)
                .Cast<object>();

            towersInCurrentTowersetView.SetData(
                towersInCurrentTowersetViewData,
                Conversions.ConvertTowerToken);
        }

        #endregion

        #region Data View Event Methods

        private void OnEnable()
        {
            availableTowersView.OnSelect += OnAvailableTowerSelected;
            availableTowersView.OnDeselect += OnAvailableTowerDeselected;
            
            towersInCurrentTowersetView.OnSelect += OnTowerInCurrentTowersetSelected;
            towersInCurrentTowersetView.OnDeselect += OnTowerInCurrentTowersetDeselected;

            availableTowersView.Deselect();
            towersInCurrentTowersetView.Deselect();
        }

        private void OnDisable()
        {
            availableTowersView.Deselect();
            towersInCurrentTowersetView.Deselect();

            gameObject.SetActive(false);

            availableTowersView.OnSelect -= OnAvailableTowerSelected;
            availableTowersView.OnDeselect -= OnAvailableTowerDeselected;

            towersInCurrentTowersetView.OnSelect -= OnTowerInCurrentTowersetSelected;
            towersInCurrentTowersetView.OnDeselect -= OnTowerInCurrentTowersetDeselected;
        }

        private void OnAvailableTowerSelected()
        {
            addTowerButton.interactable = true;
        }

        private void OnAvailableTowerDeselected()
        {
            addTowerButton.interactable = false;
        }

        private void OnTowerInCurrentTowersetSelected()
        {
            removeTowerButton.interactable = true;
        }

        private void OnTowerInCurrentTowersetDeselected()
        {
            removeTowerButton.interactable = false;
        }

        #endregion

        #region UI Methods

        public void OnTowersetNameInputValueChanged()
        {
            string newValue = towersetNameInput.text;

            if (newValue == oldTowersetName)
                return;

            if (string.IsNullOrEmpty(newValue))
            {
                WarnFeedback(towersetNameCannotBeEmpty);
                return;
            }

            TowersetContext.Identity possibleTowersetIdentity = new(newValue, userCookie.UserId);

            bool towersetExists = databaseManager
                .TowersetManager.TrackedTowersets
                .ContainsKey(possibleTowersetIdentity);

            confirmRenamingButton.interactable = !towersetExists;
            
            bool renaming = confirmRenamingButton.gameObject.activeInHierarchy;
            bool inCreateMode = screenTitle.text == titleForCreating;

            saveButton.interactable = (inCreateMode && !towersetExists) || (!inCreateMode && !renaming);

            if(towersetExists && (inCreateMode || renaming))
            {
                WarnFeedback(towersetAlreadyExists);
            }
            else
            {
                feedbackText.text = string.Empty;
            }
        }

        public void AddSelectedAvailableTower()
        {
            if (availableTowersView.SelectedEntry == null)
                return;

            int selectedIndex = availableTowersView.SelectedEntry.Index;
            TowerToken selectedTower = availableTowersView.Data[selectedIndex]
                as TowerToken;

            if (selectedTower == null)
            {
                Debug.LogWarning("Available towers view doesn't contain TowerTokens.");
                return;
            }

            if (towersInCurrentTowerset.Contains(selectedTower))
                return;

            towersInCurrentTowerset.Add(selectedTower);
            UpdateTowersInCurrentTowersetView(towersInCurrentTowerset);
        }

        public void RemoveSelectedTowerInCurrentTowerset()
        {
            if (towersInCurrentTowersetView.SelectedEntry == null)
                return;
            
            int selectedIndex = towersInCurrentTowersetView.SelectedEntry.Index;
            TowerToken selectedTower = towersInCurrentTowersetView.Data[selectedIndex]
                as TowerToken;

            if (selectedTower == null)
            {
                Debug.LogWarning("Towers in current towerset view doesn't contain TowerTokens.");
                return;
            }

            if (!towersInCurrentTowerset.Contains(selectedTower))
                return;

            towersInCurrentTowerset.Remove(selectedTower);
            towersInCurrentTowersetView.RemoveEntry(selectedIndex);
        }

        public void SaveTowerset()
        {
            string towersetToSave = towersetNameInput.text;

            if(string.IsNullOrEmpty(towersetToSave))
            {
                WarnFeedback(towersetNameCannotBeEmpty);
                return;
            }

            SaveTowersetWithName(towersetToSave, onCompleted: (result) =>
            {
                if(!result.WasSuccessful)
                {
                    ErrorFeedback(databaseError);
                    return;
                }
            });
        }

        private void SaveTowersetWithName(string towersetName, Action<Result> onCompleted)
        {
            TowersetContext.Identity currentTowersetIdentity
                = new(towersetName, userCookie.UserId);

            TowersetInfo towersetInfo = new(
                towersInCurrentTowerset
                .OrderBy(token => token.IndexInLibrary));

            bool validity = towersetInfo.Validity();

            databaseManager.TowersetManager.Set(
               currentTowersetIdentity,
               towersetInfo, validity,
            onCompleted: (result) =>
            {
                if (!result.WasSuccessful)
                {
                    Debug.LogWarning(result.Message);
                    ErrorFeedback(databaseError);
                    onCompleted?.Invoke(result);
                    return;
                }

                SetScreenMode(towersetName);
                onCompleted?.Invoke(Result.OK);
            });
        }

        public void StartRenamingCurrentTowerset()
        {
            oldTowersetName = towersetNameInput.text;
        }

        public void FinishRenamingCurrentTowerset()
        {
            string newName = towersetNameInput.text;

            if(newName == oldTowersetName)
                return;

            SaveTowersetWithName(newName, onCompleted: (result) =>
            {
                if(!result.WasSuccessful)
                {
                    ErrorFeedback(databaseError);
                    return;
                }

                SetScreenMode(newName);
                DeleteTowersetWithName(oldTowersetName);
            });
        }

        public void CancelRenamingOfCurrentTowerset()
        {
            SetScreenMode(oldTowersetName);
        }

        private void DeleteTowersetWithName(string towersetName)
        {
            TowersetContext.Identity identity = new(towersetName, userCookie.UserId);

            databaseManager.TowersetManager.Delete(identity, null);
        }

        #endregion
    }
}