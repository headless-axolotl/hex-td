using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Data.Towerset;
using System.Linq;

namespace Lotl.Data
{
    using TowersetIdentity = TowersetContext.Identity;

    public class TowersetManager
    {
        private readonly TowersetContext context;
        private readonly DatabaseManager databaseManager;
        private readonly AsyncTaskHandler asyncHandler;

        private SortedDictionary<TowersetIdentity, bool> trackedTowersets;
        private Dictionary<TowersetIdentity, TowersetInfo> cachedTowersetInfos;

        public IReadOnlyDictionary<TowersetIdentity, bool> TrackedTowersets => trackedTowersets;

        public TowersetManager(
            TowersetContext context,
            DatabaseManager databseManager,
            AsyncTaskHandler asyncHandler,
            Action onComplete)
        {
            this.context = context;
            this.databaseManager = databseManager;
            this.asyncHandler = asyncHandler;

            cachedTowersetInfos = new();

            Initialize(onComplete);
        }

        private void Initialize(Action onComplete)
        {
            asyncHandler.HandleTask(context.ReadAll(), (entries) => 
            {
                trackedTowersets = new(entries.ToDictionary(
                    entry => entry.identity,
                    entry => entry.isValid));
                onComplete?.Invoke();
            });
        }

        public bool TowersetExists(TowersetIdentity identity)
            => trackedTowersets.ContainsKey(identity);

        public void Set(TowersetIdentity identity, TowersetInfo info, bool validity, Action onComplete)
        {
            trackedTowersets[identity] = validity;
            cachedTowersetInfos[identity] = info;

            byte[] data = TowersetInfo.Serialize(info);

            asyncHandler.HandleTask(context.Set(identity, new(validity, data)), onComplete);
        }

        public void GetTowersetInfo(TowersetIdentity identity, Action<TowersetInfo> onComplete)
        {
            if (cachedTowersetInfos.TryGetValue(identity, out var info))
            {
                onComplete?.Invoke(info);
                return;
            }

            asyncHandler.HandleTask(context.ReadData(identity), (entry) =>
            {
                TowersetInfo info = TowersetInfo.Deserialize(entry.data, databaseManager.TowerTokenLibrary);
                cachedTowersetInfos[identity] = info;
                onComplete?.Invoke(info);
            });
        }

        public void Delete(TowersetIdentity identity, Action onComplete)
        {
            asyncHandler.HandleTask(context.Delete(identity), onComplete);
        }
    }
}
