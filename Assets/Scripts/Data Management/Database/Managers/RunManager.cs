using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

using Lotl.Utility.Async;
using Lotl.Data.Runs;

namespace Lotl.Data
{
    using RunIdentity = RunContext.Identity;
    using Result = AsyncTaskResult;

    public class RunManager : BaseManager
    {
        private readonly RunContext context;
        private readonly DatabaseManager databaseManager;
        private readonly AsyncTaskProcessor asyncProcessor;

        private HashSet<RunIdentity> trackedRuns;
        private Dictionary<RunIdentity, RunData> cachedRunData;

        public IReadOnlyCollection<RunIdentity> TrackedRuns => trackedRuns;

        public RunManager(
            RunContext context,
            DatabaseManager databaseManager,
            AsyncTaskProcessor asyncHandler)
        {
            this.context = context;
            this.databaseManager = databaseManager;
            this.asyncProcessor = asyncHandler;

            trackedRuns = null;
            cachedRunData = new();
        }

        public void Initialize(Action<Result> onComplete)
        {
            if(IsInitialized)
            {
                onComplete?.Invoke(Result.OK);
                return;
            }

            var readAll = context.ReadAllAsync();

            asyncProcessor.ProcessTask(readAll, onComplete, onSuccess: (entries) =>
            {
                trackedRuns = new(entries);
                Initialize();
            });
        }

        public bool RunExists(RunIdentity identity)
        {
            if (!ProperlyInitialized(null)) return false;
            
            return trackedRuns.Contains(identity);
        }

        public void Create(
            RunIdentity identity,
            RunData runData,
            Action<Result> onComplete)
        {
            if (!ProperlyInitialized(onComplete)) return;

            if (RunExists(identity))
            {
                onComplete?.Invoke(Result.OK);
                return;
            }

            byte[] data = RunData.Serialize(runData);

            Task set = context.SetAsync(identity, data);

            asyncProcessor.ProcessTask(set, onComplete, onSuccess: () =>
            {
                trackedRuns.Add(identity);
                cachedRunData[identity] = runData;
            });
        }

        public void GetRunData(
            RunIdentity identity,
            Action<Result, RunData> onComplete)
        {
            if (!ProperlyInitialized(onComplete)) return;

            if (cachedRunData.TryGetValue(identity, out var runData))
            {
                onComplete?.Invoke(Result.OK, runData);
                return;
            }

            var readData = context.ReadDataAsync(identity);

            asyncProcessor.ProcessTask(readData, onComplete, onSuccess: (data) =>
            {
                RunData runData = RunData.Deserialize(
                    data,
                    databaseManager.TowerTokenLibrary);
                
                cachedRunData[identity] = runData;

                return runData;
            });
        }

        public void Delete(
            RunIdentity identity,
            Action<Result> onComplete)
        {
            if (!ProperlyInitialized(onComplete)) return;

            Task delete = context.DeleteAsync(identity);

            asyncProcessor.ProcessTask(delete, onComplete, onSuccess: () =>
            {
                trackedRuns.Remove(identity);
                cachedRunData.Remove(identity);
            });
        }
    }
}
