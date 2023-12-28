using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lotl.UI
{
    public class DataView : MonoBehaviour
    {
        #region Events

        public event Action OnSelect;

        public event Action OnDeselect;

        #endregion

        #region Properties

        [SerializeField] private GameObject dataGridEntryViewPrefab;
        [SerializeField] private VerticalLayoutGroup entryParent;
        private List<DataEntryView> entryViews = new();
        private Func<object, Entry> visualizer = (o) => { return new(o.ToString(), string.Empty); };
        private List<object> data = new();

        public DataEntryView SelectedEntry { get; private set; }

        public IReadOnlyList<object> Data => data;

        #endregion

        #region Methods

        public void EntryWasSelected(DataEntryView entry)
        {
            if(entry == SelectedEntry)
            {
                Deselect();
                return;
            }

            if(SelectedEntry != null)
            {
                SelectedEntry.Highlight(false);
            }
            
            SelectedEntry = entry;
            SelectedEntry.Highlight();
            OnSelect?.Invoke();
        }

        public void Deselect()
        {
            if (SelectedEntry != null)
            {
                SelectedEntry.Highlight(false);
            }

            SelectedEntry = null;
            OnDeselect?.Invoke();
        }

        public void AddEntry(object entry)
        {
            Deselect();
            data.Add(entry);
            UpdateEntries();
        }

        public void AddEntry(object entry, int index)
        {
            Deselect();
            if (index == data.Count || data.Count == 0) data.Add(entry);
            else data.Insert(index, entry);
            UpdateEntries();
        }

        public void RemoveEntry(int index)
        {
            Deselect();
            data.RemoveAt(index);
            UpdateEntries();
        }
        
        public void ClearEntries()
        {
            Deselect();
            data.Clear();
            UpdateEntries();
        }

        public void SetVisualizer(Func<object, Entry> visualizer)
        {
            this.visualizer = visualizer;
            UpdateEntries();
        }

        public void SetData(IEnumerable<object> data, Func<object, Entry> visualizer)
        {
            this.data.Clear();
            this.data.AddRange(data);
            this.visualizer = visualizer;
            UpdateEntries();
        }

        private void UpdateEntries()
        {
            Deselect();
            UpdateEntryObjects();
            for(int i = 0; i < data.Count; i++)
            {
                entryViews[i].Visualize(visualizer(data[i]));
            }
        }

        private void UpdateEntryObjects()
        {
            int count = data.Count;
            UpdateEntryObjectCount(count);

            for (int i = 0; i < count; i++)
                entryViews[i].gameObject.SetActive(true);
            for(int i = count; i < entryViews.Count; i++)
                entryViews[i].gameObject.SetActive(false);

            ResizeView(count);
        }

        private void UpdateEntryObjectCount(int count)
        {
            if (count < entryViews.Count)
                return;
            
            for (int i = entryViews.Count; i < count; i++)
            {
                if (!Instantiate(dataGridEntryViewPrefab, entryParent.transform)
                    .TryGetComponent<DataEntryView>(out var entryView))
                {
                    Debug.LogError("Prefab is missing DataGridEntryView component!");
                    break;
                }
                entryView.Initialize(this, i);
                entryViews.Add(entryView);
            }
        }

        private void ResizeView(int count)
        {
            RectTransform entryTransform = dataGridEntryViewPrefab.transform as RectTransform;
            float entryHeight = entryTransform.rect.height;
            
            float viewSize = entryParent.padding.top + entryParent.padding.bottom;
            viewSize += entryParent.spacing * (count - 1);
            viewSize += entryHeight * count;
            
            RectTransform parentTransform = entryParent.transform as RectTransform;
            parentTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, viewSize);
        }

        #endregion

        public struct Entry
        {
            public string name;
            public string description;

            public Entry(string name, string description)
            {
                this.name = name;
                this.description = description;
            }
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            if (dataGridEntryViewPrefab == null) return;
            if (!dataGridEntryViewPrefab.TryGetComponent<DataEntryView>(out var _))
            {
                dataGridEntryViewPrefab = null;
                Debug.LogWarning("Prefab must have a DataGridEntryView component!");
            }
        }

#endif
    }
}
