using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using Lotl.Generic.Variables;
using Lotl.Data.Runs;

namespace Lotl.Gameplay
{
    public class RunInfoTracker : MonoBehaviour
    {
        [Header("Tracked Data")]
        [SerializeField] private IntVariable resources;
        [SerializeField] private RunDataObject crossSceneData;
        [Header("UI")]
        [SerializeField] private TMP_Text resourcesLabel;
        [SerializeField] private TMP_Text waveIndexLabel;

        private void OnEnable()
        {
            resources.Changed += UpdateResources;
            UpdateWaveIndex();
            UpdateResources(resources.Value);
        }

        private void OnDisable()
        {
            resources.Changed -= UpdateResources;
        }

        public void UpdateWaveIndex()
        {
            waveIndexLabel.text = crossSceneData.Data.RunInfo.WaveIndex.ToString();
        }

        private void UpdateResources(int amount)
        {
            resourcesLabel.text = amount.ToString();
        }
    }
}