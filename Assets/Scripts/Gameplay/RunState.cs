using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Runtime;
using Lotl.Generic.Variables;
using Lotl.Data.Runs;

namespace Lotl.Gameplay
{
    [System.Serializable]
    public class RunState
    {
        [SerializeField] private IntVariable resources;
        [SerializeField] private AutounitRuntimeSet towers;

        public int Resources => resources.Value;
        public AutounitRuntimeSet Towers => towers;

        public void Save(RunInfo into)
        {
            if (!IsValid()) return;
            into.Resources = resources.Value;
            RunInfo.ExtractTowerInfo(into, towers);
        }

        public void Load(RunInfo from, TowerBuilder builder)
        {
            if (!IsValid()) return;
            resources.Value = from.Resources;
            foreach (TowerInfo towerInfo in from.TowersData)
                builder.TryCreate(towerInfo, out var _);
        }

        private bool IsValid()
        {
            if (towers == null)
            {
                Debug.LogError("Run state is missing a reference to a tower runtime set!");
                return false;
            }
            if (resources == null)
            {
                Debug.LogError("Run state is missing a reference to an int variable!");
                return false;
            }
            return true;
        }
    }
}
