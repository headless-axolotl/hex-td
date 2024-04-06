using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

using Lotl.Utility.Async;
using Lotl.Data.Towerset;

namespace Lotl.Data
{
    using TowersetIdentity = TowersetContext.Identity;
    using Result = AsyncTaskResult;

    public class TowersetManager : BaseManager
    {
        private readonly TowersetContext context;
        private readonly DatabaseManager databaseManager;
        private readonly AsyncTaskProcessor asyncProcessor;

        private SortedDictionary<TowersetIdentity, bool> trackedTowersets;
        private Dictionary<TowersetIdentity, TowersetInfo> cachedTowersetInfos;

        public IReadOnlyDictionary<TowersetIdentity, bool> TrackedTowersets => trackedTowersets;

        public TowersetManager(
            TowersetContext context,
            DatabaseManager databaseManager,
            AsyncTaskProcessor asyncProcessor)
        {
            this.context = context;
            this.databaseManager = databaseManager;
            this.asyncProcessor = asyncProcessor;

            trackedTowersets = null;
            cachedTowersetInfos = new();
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
                trackedTowersets = new(entries.ToDictionary(
                        entry => entry.identity,
                        entry => entry.isValid));

                Initialize();
            });
        }

        public bool TowersetExists(TowersetIdentity identity)
        {
            if (!ProperlyInitialized(null)) return false;
            
            return trackedTowersets.ContainsKey(identity);
        }

        public void Set(
            TowersetIdentity identity,
            TowersetInfo info,
            bool validity,
            Action<Result> onCompleted)
        {
            if (!ProperlyInitialized(onCompleted)) return;

            byte[] data = TowersetInfo.Serialize(info);

            Task set = context.SetAsync(identity, new(validity, data));

            asyncProcessor.ProcessTask(set, onCompleted, onSuccess: () =>   
            {
                trackedTowersets[identity] = validity;
                cachedTowersetInfos[identity] = info;
            });
        }

        public void GetTowersetInfo(
            TowersetIdentity identity,
            Action<Result, TowersetInfo> onCompleted)
        {
            if (!ProperlyInitialized(onCompleted)) return;

            if (cachedTowersetInfos.TryGetValue(identity, out var info))
            {
                onCompleted?.Invoke(Result.OK, info);
                return;
            }

            var readData = context.ReadDataAsync(identity);

            asyncProcessor.ProcessTask(readData, onCompleted, onSuccess: (entry) =>
            {
                if(entry.data == null)
                {
                    Debug.Log("read bytes were null");
                }

                TowersetInfo info = TowersetInfo.Deserialize(
                    entry.data,
                    databaseManager.TowerTokenLibrary);

                cachedTowersetInfos[identity] = info;

                return info;
            });
        }

        public void Delete(
            TowersetIdentity identity,
            Action<Result> onCompleted)
        {
            if (!ProperlyInitialized(onCompleted)) return;

            Task delete = context.DeleteAsync(identity);

            asyncProcessor.ProcessTask(delete, onCompleted, onSuccess: () =>
            {
                trackedTowersets.Remove(identity);
                cachedTowersetInfos.Remove(identity);
            });
        }
    }
}
