using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Lotl.UI;
using Lotl.Data.Towerset;
using Lotl.Utility;

namespace Lotl.Data.Menu
{
    public class TowersMenuScreen : MenuScreen
    {
        #region Properties

        [Header("UI")]
        [SerializeField] private DataView createdTowersetsView;
        [SerializeField] private DataView availableTowersView;
        [SerializeField] private DataView towersInCurrentTowersetView;

        [SerializeField] private Button createButton;
        [SerializeField] private Button updateButton;
        [SerializeField] private Button deleteButton;

        [SerializeField] private TMP_InputField nameInputField;

        private SortedDictionary<string, TowersetTable.Entry> trackedTowersets;
        private HashSet<string> towersInCurrentTowerset = new();
        private TowersetTable.Entry selectedTowerset;

        #endregion

        #region Methods

        private void Start()
        {
            Initialize();
        }

        private void OnDisable()
        {
            ClearState();
        }

        private void Initialize()
        {
            trackedTowersets = new(dataManager.TowersetTable
                .ReadAll().ToDictionary(e => e.id));
            towersInCurrentTowerset = new();

            createdTowersetsView.ClearEntries();
            createdTowersetsView.OnSelect += OnTowersetSelected;

            availableTowersView.SetData(
                dataManager.TowerTokenLibrary.TowerTokens.Select(item => (object)item).ToList(),
                Conversions.ConvertTowerToken);

            towersInCurrentTowersetView.SetVisualizer(
               Conversions.ConvertTowerToken);

            ClearState();
        }

        #region Helper Methods

        private void SetCreation(bool unlock = true)
        {
            nameInputField.interactable =
            createButton.interactable = unlock;

            updateButton.interactable =
            deleteButton.interactable = !unlock;
        }
        
        private void ClearState()
        {
            SetCreation();

            nameInputField.text = string.Empty;
            NameCheck();

            createdTowersetsView.SetData(
                trackedTowersets.Values.Select(item => (object)item).ToList(),
                Conversions.ConvertTowersetTableEntry);
            
            availableTowersView.Deselect();
            towersInCurrentTowersetView.ClearEntries();
            towersInCurrentTowerset.Clear();
        }

        private void FillInputs()
        {
            SetCreation(false);

            nameInputField.text = selectedTowerset.id;

            TowersetInfo towersetInfo = dataManager.GetTowersetInfo(selectedTowerset.id);

            towersInCurrentTowersetView.SetData(                
                towersetInfo.TowerTokens.Select(item => (object)item).ToList(),
                Conversions.ConvertTowerToken);
        }

        #endregion

        #region UI Methods

        public void OnTowersetSelected(object _, EventArgs __)
        {
            selectedTowerset = trackedTowersets[createdTowersetsView.SelectedEntry.EntryName];
            SetCreation(false);
            FillInputs();
        }

        public void NameCheck()
        {
            if (trackedTowersets.ContainsKey(nameInputField.text))
                 createButton.interactable = false;
            else createButton.interactable = true;
        }

        #endregion

        #region Logic

        public void Set()
        {
            string towersetId = nameInputField.text;

            if(string.IsNullOrEmpty(towersetId)) return;

            bool validity = true;
            
            if(!trackedTowersets.ContainsKey(towersetId))
            {
                trackedTowersets.Add(towersetId, new(towersetId, validity));
            }
            else
            {
                #warning Do something with validity
            }

            TowersetInfo info = new(towersInCurrentTowersetView.Data.Select(item => (TowerToken)item).ToList());
            dataManager.TowersetTable.Set(towersetId, validity, TowersetInfo.Serialize(info));

            dataManager.CachedTowersetInfos[towersetId] = info;

            ClearState();
        }

        public void Delete()
        {
            string towersetId = nameInputField.text;

            trackedTowersets.Remove(towersetId);
            dataManager.TowersetTable.Delete(towersetId);

            ClearState();
        }

        public void Add()
        {
            var selectedEntry = availableTowersView.SelectedEntry;
            if (selectedEntry == null || towersInCurrentTowerset.Contains(selectedEntry.EntryName))
                return;
            
            TowerToken tokenToAdd = availableTowersView.Data[availableTowersView.SelectedEntry.Index]
                as TowerToken;

            List<int> indeces = towersInCurrentTowersetView.Data
                .Select(item => ((TowerToken)item).IndexInLibrary)
                .ToList();

            int index = 0;

            if (indeces.Count != 0)
            {
                index = Algorithms.BinarySearch(indeces, tokenToAdd.IndexInLibrary);
                    
                int libraryIndexOfBinarySearchedToken = 
                    ((TowerToken)towersInCurrentTowersetView.Data[index]).IndexInLibrary;
                    
                if(libraryIndexOfBinarySearchedToken < tokenToAdd.IndexInLibrary)
                    index++;
            }

            towersInCurrentTowerset.Add(selectedEntry.EntryName);
                
            towersInCurrentTowersetView.AddEntry(tokenToAdd, index);
        }

        public void Remove()
        {
            var selectedEntry = towersInCurrentTowersetView.SelectedEntry;
            
            if (selectedEntry == null)
                return;
            
            towersInCurrentTowerset.Remove(selectedEntry.EntryName);
            towersInCurrentTowersetView.RemoveEntry(selectedEntry.Index);
        }

        public void Clear()
        {
            ClearState();
        }

        #endregion

        #endregion
    }
}
