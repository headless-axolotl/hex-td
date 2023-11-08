using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

using Lotl.Utility;
using Lotl.Data.Runs;

namespace Lotl.Data
{
    using RunIdentity = RunContext.Identity;

    public class RunManager : BaseManager
    {
        private readonly RunContext context;
        private readonly DatabaseManager databaseManager;
        private readonly AsyncTaskHandler asyncHandler;

        private HashSet<RunIdentity> trackedRuns;
        private Dictionary<RunIdentity, RunData> cachedRunData;

        public IReadOnlyCollection<RunIdentity> TrackedRuns => trackedRuns;

        public RunManager(
            RunContext context,
            DatabaseManager databaseManager,
            AsyncTaskHandler asyncHandler)
        {
            this.context = context;
            this.databaseManager = databaseManager;
            this.asyncHandler = asyncHandler;

            trackedRuns = null;
            cachedRunData = new();
        }

        public void Initialize(Action<bool> onComplete)
        {
            if (!ProperlyInitialized(onComplete)) return;

            var readAll = context.ReadAll();

            asyncHandler.HandleTask(readAll, (entries, isSuccessful) =>
            {
                if(!isSuccessful)
                {
                    onComplete?.Invoke(false);
                    return;
                }

                trackedRuns = new(entries);

                Initialize();
                
                onComplete?.Invoke(true);
            });
        }

        public bool RunExists(RunIdentity identity)
        {
            if (!ProperlyInitialized(null)) return false;
            return trackedRuns.Contains(identity);
        }

        public void Create(RunIdentity identity, RunData runData, Action<bool> onComplete)
        {
            if (!ProperlyInitialized(onComplete)) return;

            if (RunExists(identity)) return;

            byte[] data = RunData.Serialize(runData);

            trackedRuns.Add(identity);
            cachedRunData[identity] = runData;

            Task set = context.Set(identity, data);

            asyncHandler.HandleTask(set, onComplete);
        }

        public void GetRunData(RunIdentity identity, Action<RunData, bool> onComplete)
        {
            if (!CheckInitialization(onComplete)) return;

            if (cachedRunData.TryGetValue(identity, out var runData))
            {
                onComplete?.Invoke(runData, true);
                return;
            }

            var readData = context.ReadData(identity);

            asyncHandler.HandleTask(readData, (data, isSuccessful) =>
            {
                RunData runData = RunData.Deserialize(data, databaseManager.TowerTokenLibrary);
                cachedRunData[identity] = runData;
                onComplete?.Invoke(runData, true);
            });
        }

        public void Delete(RunIdentity identity, Action<bool> onComplete)
        {
            if (!ProperlyInitialized(onComplete)) return;

            Task delete = context.Delete(identity);

            asyncHandler.HandleTask(delete, onComplete);
        }
    }
}
