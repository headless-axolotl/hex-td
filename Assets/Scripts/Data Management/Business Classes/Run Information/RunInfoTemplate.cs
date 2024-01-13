using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.AssetManagement;
using Lotl.Hexgrid;
using Lotl.Generic.Variables;

namespace Lotl.Data.Runs
{
    [CreateAssetMenu(fileName = "RunInfoTemplate", menuName = "Lotl/Data/Run Info Template")]
    public class RunInfoTemplate : ScriptableObject
    {
        [SerializeField] private string templateName;
        [Space] [TextArea]
        [SerializeField] private string templateDescription;
        [Space]
        [SerializeField] private IntReference resources;
        [SerializeField] private IntReference waveIndex;
        [SerializeField] private List<ReferencedTowerInfo> towersData;

        public string TemplateName => templateName;
        public string TemplateDescription => templateDescription;
        
        public RunInfo CreateRunInfo()
        {
            var towerInfos = towersData.Select(
                rti => new TowerInfo()
                {
                    BookIndex = rti.reference.BookIndex,
                    PrefabIndex = rti.reference.PrefabIndex,
                    CurrentHealth = rti.currentHealth,
                    Position = rti.position
                });

            RunInfo runInfo = new(towerInfos)
            {
                Resources = resources,
                WaveIndex = waveIndex
            };

            return runInfo;
        }

        [System.Serializable]
        public struct ReferencedTowerInfo
        {
            public PrefabReference reference;
            public FloatReference currentHealth;
            public Hex position;
        }
    }
}
