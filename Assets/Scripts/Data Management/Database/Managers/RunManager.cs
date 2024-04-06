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
            AsyncTaskProcessor asyncProcessor)
        {
            this.context = context;
            this.databaseManager = databaseManager;
            this.asyncProcessor = asyncProcessor;

            trackedRuns = null;
            cachedRunData = new();
        }

        public void Initialize(Action<Result> onCompleted)
        {
            if(IsInitialized)
            {
                onCompleted?.Invoke(Result.OK);
                return;
            }

            var readAll = context.ReadAllAsync();

            asyncProcessor.ProcessTask(readAll, onCompleted, onSuccess: (entries) =>
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

        public void Set(
            RunIdentity identity,
            RunData runData,
            Action<Result> onCompleted)
        {
            if (!ProperlyInitialized(onCompleted)) return;

            byte[] data = RunData.Serialize(runData);

            Task set = context.SetAsync(identity, data);

            asyncProcessor.ProcessTask(set, onCompleted, onSuccess: () =>
            {
                trackedRuns.Add(identity);
                cachedRunData[identity] = runData;
            });
        }

        public void GetRunData(
            RunIdentity identity,
            Action<Result, RunData> onCompleted)
        {
            if (!ProperlyInitialized(onCompleted)) return;

            if (cachedRunData.TryGetValue(identity, out var runData))
            {
                onCompleted?.Invoke(Result.OK, runData);
                return;
            }

            var readData = context.ReadDataAsync(identity);

            asyncProcessor.ProcessTask(readData, onCompleted, onSuccess: (data) =>
            {
                RunData runData = RunData.Deserialize(
                    data,
                    databaseManager.TowerTokenLibrary);
                runData.RunId = identity.name;
                
                cachedRunData[identity] = runData;

                return runData;
            });
        }

        public void Delete(
            RunIdentity identity,
            Action<Result> onCompleted)
        {
            if (!ProperlyInitialized(onCompleted)) return;

            Task delete = context.DeleteAsync(identity);

            asyncProcessor.ProcessTask(delete, onCompleted, onSuccess: () =>
            {
                trackedRuns.Remove(identity);
                cachedRunData.Remove(identity);
            });
        }
    }
}
