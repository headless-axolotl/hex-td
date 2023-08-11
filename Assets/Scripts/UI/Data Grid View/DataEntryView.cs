using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Lotl.UI
{
    [RequireComponent(typeof(Outline))]
    public class DataEntryView : MonoBehaviour
    {
        [SerializeField] private TMP_Text entryName;
        [SerializeField] private TMP_Text entryDescription;
        private Outline outline;
        private DataView parentDataGridView;

        public string EntryName => entryName.text;
        public string EntryDescription => entryDescription.text;

        private void Awake()
        {
            outline = GetComponent<Outline>();
        }

        public int Index { get; private set; }

        public void Initialize(DataView dataGridView, int index)
        {
            parentDataGridView = dataGridView;
            Index = index;
        }

        public void Visualize(DataView.Entry entry)
        {
            entryName.text = entry.name;
            entryDescription.text = entry.description;
        }

        public void Select()
        {
            parentDataGridView.EntryWasSelected(this);
        }

        public void Highlight(bool state = true)
        {
            outline.enabled = state;
        }
    }
}

