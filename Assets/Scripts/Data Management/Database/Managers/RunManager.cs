using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Data.Runs;

namespace Lotl.Data
{
    using RunIdentity = RunContext.Identity;

    public class RunManager
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
            AsyncTaskHandler asyncHandler,
            Action onComplete)
        {
            this.context = context;
            this.databaseManager = databaseManager;
            this.asyncHandler = asyncHandler;

            cachedRunData = new();
            
            Initialize(onComplete);
        }

        private void Initialize(Action onComplete)
        {
            asyncHandler.HandleTask(context.ReadAll(), (entries) =>
            {
                trackedRuns = new(entries);
                onComplete?.Invoke();
            });
        }

        public bool RunExists(RunIdentity identity)
            => trackedRuns.Contains(identity);

        public void Create(RunIdentity identity, RunData runData, Action onComplete)
        {
            if (RunExists(identity)) return;

            byte[] data = RunData.Serialize(runData);

            trackedRuns.Add(identity);
            cachedRunData[identity] = runData;
            
            asyncHandler.HandleTask(context.Set(identity, data), onComplete);
        }

        public void GetRunData(RunIdentity identity, Action<RunData> onComplete)
        {
            if (cachedRunData.TryGetValue(identity, out var runData))
            {
                onComplete?.Invoke(runData);
                return;
            }

            asyncHandler.HandleTask(context.ReadData(identity), (data) =>
            {
                RunData runData = RunData.Deserialize(data, databaseManager.TowerTokenLibrary);
                cachedRunData[identity] = runData;
                onComplete?.Invoke(runData);
            });
        }

        public void Delete(RunIdentity identity, Action onComplete)
        {
            asyncHandler.HandleTask(context.Delete(identity), onComplete);
        }
    }
}
