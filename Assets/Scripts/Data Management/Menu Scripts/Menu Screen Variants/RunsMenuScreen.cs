using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Lotl.UI;
using Lotl.Data.Runs;
using Lotl.Data.Towerset;

namespace Lotl.Data.Menu
{
    public class RunsMenuScreen : MenuScreen
    {
        #region Properties
        
        [Header("UI")]
        [SerializeField] private DataView createdRunsView;
        [SerializeField] private DataView createdRunTowersetPreview;
        [SerializeField] private DataView creationSubmenuTowersetPreview;

        [SerializeField] private Button openSubmenuButton;
        [SerializeField] private Button createButton;
        [SerializeField] private Button playButton;
        [SerializeField] private Button deleteButton;

        [SerializeField] private TMP_InputField nameInput;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private TMP_Dropdown runTemplateDropdown;
        [SerializeField] private TMP_Dropdown towersetDropdown;

        [SerializeField] private GameObject creationSubmenu;

        private HashSet<string> trackedRuns;
        private Dictionary<string, RunData> cachedRunDatas;
        private string selectedRun;

        #endregion

        #region Methods

        private void Start()
        {
            Initialize();
        }

        private void OnEnable()
        {
            UpdateTowersetDropdown();
        }

        private void OnDisable()
        {
            SetSubmenu(false);
        }

        private void Initialize()
        {
            trackedRuns = new(dataManager.RunTable.ReadAll());

            cachedRunDatas = new();

            createdRunsView.ClearEntries();
            createdRunsView.OnSelect += SelectRun;
            SetCreatedRunsViewData();

            InitializeRunTemplateDropdown();

            DeselectRun();
        }

        private void SetCreatedRunsViewData()
        {
            createdRunsView.SetData(
                trackedRuns.Select(item => (object)item).ToList(),
                Conversions.ConvertWithToString);
        }

        #region Submenu

        public void SetSubmenu(bool open = true)
        {
            DeselectRun();
            ClearSubmenu();
            ValidateName();
            creationSubmenu.SetActive(open);
        }

        public void ClearSubmenu()
        {
            nameInput.text = string.Empty;
            runTemplateDropdown.value = 0;
            towersetDropdown.value = 1; towersetDropdown.value = 0;
        }

        private void InitializeRunTemplateDropdown()
        {
            runTemplateDropdown.ClearOptions();
            foreach (var runTemplate in dataManager.RunTemplates)
            {
                runTemplateDropdown.options.Add(new(runTemplate.TemplateName));
            }

            // If the first entry is deleted while it is selected it will show
            // as blank in the dropdwon until it is changed.
            // I assume that the dropdown does not update the preview
            // if it is changed to the same number, so I change it to an arbitrary value
            // before returning it to the first element.
            runTemplateDropdown.value = 1; runTemplateDropdown.value = 0;
        }

        private void UpdateTowersetDropdown()
        {
            towersetDropdown.ClearOptions();
            foreach (var entry in dataManager.TowersetTable.ReadAll().OrderBy(e => e.id))
            {
                if (!entry.isValid) continue;
                towersetDropdown.options.Add(new(entry.id));
            }
            towersetDropdown.value = 1; towersetDropdown.value = 0;

            openSubmenuButton.interactable = towersetDropdown.options.Count != 0;
        }

        public void ValidateName()
        {
            createButton.interactable = !(
                trackedRuns.Contains(nameInput.text)
                || string.IsNullOrEmpty(nameInput.text));
        }

        public void OnTowersetPickerValueChanged(int value)
        {
            string selectedTowersetInfoId = towersetDropdown.options[value].text;
            TowersetInfo towersetInfo = dataManager.GetTowersetInfo(selectedTowersetInfoId);

            creationSubmenuTowersetPreview.SetData(
                towersetInfo.TowerTokens.Select(item => (object)item).ToList(),
                Conversions.ConvertTowerToken);
        }

        public void OnRunTemplatePickerValueChanged(int value)
        {
            descriptionText.text = dataManager.RunTemplates[value].TemplateDescription;
        }

        #endregion

        #region Main Menu

        public void SelectRun(object _, EventArgs __)
        {
            selectedRun = createdRunsView.SelectedEntry.EntryName;

            RunData runData = GetRunData(selectedRun);

            createdRunTowersetPreview.SetData(
                runData.TowersetInfo.TowerTokens.Select(item => (object)item).ToList(),
                Conversions.ConvertTowerToken);

            playButton.interactable   =
            deleteButton.interactable = true;
        }

        private void DeselectRun()
        {
            selectedRun = string.Empty;

            createdRunsView.Deselect();
            createdRunTowersetPreview.ClearEntries();

            playButton.interactable =
            deleteButton.interactable = false;
        }

        #endregion

        #region Logic

        public void Create()
        {
            string newRunName = nameInput.text;
            
            if(trackedRuns.Contains(newRunName))
                return;

            int selectedRunTemplate = runTemplateDropdown.value;
            RunInfo templateRunInfo = dataManager.RunTemplates[selectedRunTemplate].RunInfo;

            string selectedTowersetInfoId = towersetDropdown.options[towersetDropdown.value].text;
            TowersetInfo towersetInfo = dataManager.GetTowersetInfo(selectedTowersetInfoId);

            RunData newRunData = new()
            {
                RunId = newRunName,
                RunInfo = templateRunInfo,
                TowersetInfo = towersetInfo
            };

            dataManager.RunTable.Set(newRunName, RunData.Serialize(newRunData));

            trackedRuns.Add(newRunName);

            SetCreatedRunsViewData();
        }

        public void Play()
        {
            if (selectedRun == string.Empty)
                return;
            
            dataManager.CrossSceneRunData.Data = GetRunData(selectedRun);
            
            #warning testing purposes only!
            dataManager._ChangeScene();
        }

        public void Delete()
        {
            if (selectedRun == string.Empty)
                return;

            trackedRuns.Remove(selectedRun);
            dataManager.RunTable.Delete(selectedRun);
            createdRunsView.RemoveEntry(createdRunsView.SelectedEntry.Index);
            
            DeselectRun();
        }

        private RunData GetRunData(string id)
        {
            if (!trackedRuns.Contains(id))
                return null;

            if (!cachedRunDatas.TryGetValue(id, out var runData))
            {
                runData = cachedRunDatas[id] = RunData.Deserialize(
                    dataManager.RunTable.ReadData(id),
                    dataManager.TowerTokenLibrary);
                runData.RunId = id;
            }

            return runData;
        }

        #endregion

        #endregion
    }
}
