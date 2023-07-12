using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.DataManagement
{
    public class RunDataManager : ScriptableObject
    {
        [SerializeField] private string runId;
        [SerializeField] private RunInfo currentSnapshot;

        public string RunId { get => runId; set => runId = value; }
        public RunInfo CurrentSnapshot { get => currentSnapshot; set => currentSnapshot = value; }

        // Convert current snapshot of the run info into a byte array
        // and generate such from a byte array (i.e. blob read from the database manager)
    }
}
