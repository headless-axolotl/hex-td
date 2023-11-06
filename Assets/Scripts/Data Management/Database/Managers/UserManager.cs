using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Data.Users;
using System.Linq;
using BCrypter = BCrypt.Net.BCrypt;

namespace Lotl.Data
{
    public class UserManager
    {
        private readonly UserContext context;
        private readonly DatabaseManager databaseManager;
        private readonly AsyncTaskHandler asyncHandler;

        private SortedDictionary<string, string> trackedUsers;
        private Dictionary<string, UserData> cachedUserDatas;

        public IReadOnlyDictionary<string, string> TrackedUsers => trackedUsers;

        public UserManager(
            UserContext context,
            DatabaseManager databaseManager,
            AsyncTaskHandler asyncHandler,
            Action onComplete)
        {
            this.context = context;
            this.databaseManager = databaseManager;
            this.asyncHandler = asyncHandler;

            cachedUserDatas = new();

            Initialize(onComplete);
        }

        private void Initialize(Action onComplete)
        {
            asyncHandler.HandleTask(context.ReadAll(), (entries) =>
            {
                trackedUsers = new(entries.ToDictionary(
                    entry => entry.id,
                    entry => entry.passwordHash));
                onComplete?.Invoke();
            });
        }

        public bool Validate(string userId, string password)
        {
            if(!trackedUsers.TryGetValue(userId, out var passwordHash))
                return false;

            return BCrypter.EnhancedVerify(password, passwordHash);
        }

        public void Set(string userId, string password, UserData baseData, Action onComplete)
        {
            cachedUserDatas[userId] = baseData;
            
            if (!trackedUsers.TryGetValue(userId, out string passwordHash))
            {
                passwordHash = BCrypter.EnhancedHashPassword(password);
            }

            byte[] data = UserData.Serialize(baseData);
            
            asyncHandler.HandleTask(context.Set(userId, new(passwordHash, data)), onComplete);
        }

        public void GetUserData(string userId, Action<UserData> onComplete)
        {
            if (cachedUserDatas.TryGetValue(userId, out var data))
            {
                onComplete?.Invoke(data);
                return;
            }

            asyncHandler.HandleTask(context.ReadData(userId), (entry) =>
            {
                UserData userData = UserData.Deserialize(entry.data,
                    databaseManager.TowerTokenLibrary);
                cachedUserDatas[userId] = userData;
                onComplete?.Invoke(userData);
            });
        }

        public void Delete(string userId, Action onComplete)
        {
            asyncHandler.HandleTask(context.Delete(userId), onComplete);
        }
    }
}