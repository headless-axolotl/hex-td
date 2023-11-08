using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

using Lotl.Utility;
using Lotl.Data.Towerset;

namespace Lotl.Data
{
    using TowersetIdentity = TowersetContext.Identity;

    public class TowersetManager : BaseManager
    {
        private readonly TowersetContext context;
        private readonly DatabaseManager databaseManager;
        private readonly AsyncTaskHandler asyncHandler;

        private SortedDictionary<TowersetIdentity, bool> trackedTowersets;
        private Dictionary<TowersetIdentity, TowersetInfo> cachedTowersetInfos;

        public IReadOnlyDictionary<TowersetIdentity, bool> TrackedTowersets => trackedTowersets;

        public TowersetManager(
            TowersetContext context,
            DatabaseManager databaseManager,
            AsyncTaskHandler asyncHandler)
        {
            this.context = context;
            this.databaseManager = databaseManager;
            this.asyncHandler = asyncHandler;

            trackedTowersets = null;
            cachedTowersetInfos = new();
        }

        public void Initialize(Action<bool> onComplete)
        {
            if(!ProperlyInitialized(onComplete)) return;

            var readAll = context.ReadAll();
            
            asyncHandler.HandleTask(readAll, (entries, isSuccessful) => 
            {
                if(!isSuccessful)
                {
                    onComplete?.Invoke(false);
                    return;
                }

                trackedTowersets = new(entries.ToDictionary(
                    entry => entry.identity,
                    entry => entry.isValid));
                
                Initialize();
                
                onComplete?.Invoke(true);
            });
        }

        public bool TowersetExists(TowersetIdentity identity)
        {
            if (!ProperlyInitialized(null)) return false;
            return trackedTowersets.ContainsKey(identity);
        }
            

        public void Set(TowersetIdentity identity, TowersetInfo info, bool validity, Action<bool> onComplete)
        {
            if (!ProperlyInitialized(onComplete)) return;

            trackedTowersets[identity] = validity;
            cachedTowersetInfos[identity] = info;

            byte[] data = TowersetInfo.Serialize(info);

            Task set = context.Set(identity, new(validity, data));

            asyncHandler.HandleTask(set, onComplete);
        }

        public void GetTowersetInfo(TowersetIdentity identity, Action<TowersetInfo, bool> onComplete)
        {
            if (!CheckInitialization(onComplete)) return;

            if (cachedTowersetInfos.TryGetValue(identity, out var info))
            {
                onComplete?.Invoke(info, true);
                return;
            }

            var readData = context.ReadData(identity);

            asyncHandler.HandleTask(readData, (entry, isSuccessful) =>
            {
                TowersetInfo info = TowersetInfo.Deserialize(entry.data, databaseManager.TowerTokenLibrary);
                cachedTowersetInfos[identity] = info;
                onComplete?.Invoke(info, true);
            });
        }

        public void Delete(TowersetIdentity identity, Action<bool> onComplete)
        {
            if (!ProperlyInitialized(onComplete)) return;

            Task delete = context.Delete(identity);

            asyncHandler.HandleTask(delete, onComplete);
        }
    }
}
