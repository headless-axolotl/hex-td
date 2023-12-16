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
using Lotl.Data.Towerset;
using Lotl.Data.Runs;

namespace Lotl.Data.Menu
{
    using Result = Lotl.Utility.Async.AsyncTaskResult;
    using RunIdentity = RunContext.Identity;
    using TowersetIdentity = TowersetContext.Identity;

    public class RunCreationScreen : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private DatabaseManager databaseManager;
        [SerializeField] private UserCookie userCookie;
        [SerializeField] private List<RunInfoTemplate> availableRunTemplates;

        [Header("UI")]
        [SerializeField] private TMP_InputField runNameInput;
        [SerializeField] private TMP_Dropdown towersetDropdown;
        [SerializeField] private DataView towersetPreview;
        [SerializeField] private TMP_Dropdown runTemplateDropdown;
        [SerializeField] private TMP_Text runTemplateDescription;
        [SerializeField] private Button createButton;
        [SerializeField] private TMP_Text feedbackText;

        [Header("Warning Messages")]
        [SerializeField] private Color warningColor;
        [SerializeField] private StringReference runAlreadyExists;

        [Header("Error Messages")]
        [SerializeField] private Color errorColor;
        [SerializeField] private StringReference databaseError;

        private List<TowersetIdentity> validTowersets;
        private TowersetInfo selectedTowerset;

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

        private void Activate()
        {
            feedbackText.text = string.Empty;
            
            UpdateTowersetDropdown();

            UpdateRunTemplateDropdown();
        }

        private void OnEnable()
        {
            Activate();
        }

        private void OnDisable()
        {
            gameObject.SetActive(false);
        }

        #region UI Methods

        public void OnNameChanged()
        {
            RunIdentity newRunIdentity = new(runNameInput.text, userCookie.UserId);
            
            bool runExists = databaseManager.RunManager.RunExists(newRunIdentity);

            createButton.interactable = !runExists;

            if (runExists)
            {
                WarnFeedback(runAlreadyExists);
            }
            else
            {
                feedbackText.text = string.Empty;
            }
        }

        public void OnEndNameEdit()
        {
            runNameInput.text = runNameInput.text.Trim();
        }

        private void UpdateTowersetDropdown()
        {
            validTowersets = databaseManager
                .TowersetManager.TrackedTowersets
                .Where(keyValuePair => keyValuePair.Value)
                .Select(keyValuePair => keyValuePair.Key)
                .ToList();
            
            towersetDropdown.ClearOptions();

            var options = validTowersets
                .Select(towersetIdentity => new TMP_Dropdown.OptionData(towersetIdentity.name))
                .ToList();

            towersetDropdown.AddOptions(options);
            towersetDropdown.value = 1; towersetDropdown.value = 0;
        }

        private void GetSelectedTowerset(Action<Result, TowersetInfo> onCompleted)
        {
            int selectedIndex = towersetDropdown.value;
            string towersetName = towersetDropdown.options[selectedIndex].text;

            TowersetIdentity towersetIdentity = new(towersetName, userCookie.UserId);

            databaseManager.TowersetManager.GetTowersetInfo(towersetIdentity, onCompleted);
        }

        public void OnTowersetDropdownValueChanged()
        {
            GetSelectedTowerset(onCompleted: (result, towersetInfo) =>
            {
                if(!result.WasSuccessful)
                {
                    Debug.LogWarning(result.Message);
                    ErrorFeedback(databaseError);
                    return;
                }

                selectedTowerset = towersetInfo;
                UpdateTowersetPreview(selectedTowerset);
            });
        }

        private void UpdateRunTemplateDropdown()
        {
            runTemplateDropdown.ClearOptions();

            var options = availableRunTemplates
                .Select(runInfoTemplate => new TMP_Dropdown.OptionData(runInfoTemplate.TemplateName))
                .ToList();

            runTemplateDropdown.AddOptions(options);
            runTemplateDropdown.value = 1; runTemplateDropdown.value = 0;
        }

        private RunInfoTemplate GetSelectedRunTemplate()
        {
            int selectedIndex = towersetDropdown.value;
            return availableRunTemplates[selectedIndex];
        }

        public void OnRunTemplateDropdownValueChanged()
        {
            string templateDescription = GetSelectedRunTemplate().TemplateDescription;
            runTemplateDescription.text = templateDescription;
        }

        private void UpdateTowersetPreview(TowersetInfo data)
        {
            var towersetPreviewData = data.TowerTokens.Cast<object>();
            towersetPreview.SetData(towersetPreviewData, Conversions.ConvertTowerToken);
        }

        public void CreateRun()
        {
            string runId = runNameInput.text;

            RunIdentity newRunIdentity = new(runId, userCookie.UserId);

            RunInfo runInfoFromTemplate = GetSelectedRunTemplate().CreateRunInfo();
            RunData runData = new()
            {
                RunId = runId,
                RunInfo = runInfoFromTemplate,
                TowersetInfo = selectedTowerset
            };

            databaseManager.RunManager.Create(newRunIdentity, runData, onCompleted: (result) =>
            {
                if(!result.WasSuccessful)
                {
                    Debug.LogWarning(result.Message);
                    ErrorFeedback(databaseError);
                    return;
                }

                gameObject.SetActive(false);
            });
        }

        #endregion
    }
}
