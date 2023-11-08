using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

using Lotl.Utility;
using Lotl.Data.Users;
using BCrypter = BCrypt.Net.BCrypt;

namespace Lotl.Data
{
    public class UserManager : BaseManager
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
            AsyncTaskHandler asyncHandler)
        {
            this.context = context;
            this.databaseManager = databaseManager;
            this.asyncHandler = asyncHandler;

            trackedUsers = null;
            cachedUserDatas = new();
        }

        public void Initialize(Action<bool> onComplete)
        {
            var readAll = context.ReadAll();

            asyncHandler.HandleTask(readAll, (entries, isSuccessful) =>
            {
                if(!isSuccessful)
                {
                    onComplete?.Invoke(false);
                    return;
                }

                trackedUsers = new(entries.ToDictionary(
                    entry => entry.id,
                    entry => entry.passwordHash));

                Initialize();

                onComplete?.Invoke(true);
            });
        }

        public bool Validate(string userId, string password)
        {
            if (!ProperlyInitialized(null)) return false;

            if (!trackedUsers.TryGetValue(userId, out var passwordHash))
                return false;

            return BCrypter.EnhancedVerify(password, passwordHash);
        }

        public void TryCreateUser(string userId, string password, UserData baseData, Action<bool> onComplete)
        {
            if (!ProperlyInitialized(onComplete)) return;

            if (trackedUsers.ContainsKey(userId))
            {
                onComplete?.Invoke(false);
                return;
            }

            string passwordHash = BCrypter.EnhancedHashPassword(password);
            byte[] data = UserData.Serialize(baseData);

            Task set = context.Set(userId, new(passwordHash, data));

            asyncHandler.HandleTask(set, onComplete);
        }

        public void TryUpdatePassword(string userId, string oldPassword, string newPassword, Action<bool> onComplete)
        {
            if (!ProperlyInitialized(onComplete)) return;

            if (!Validate(userId, oldPassword))
            {
                onComplete?.Invoke(false);
                return;
            }

            string newPasswordHash = BCrypter.EnhancedHashPassword(newPassword);

            var readData = context.ReadData(userId);

            asyncHandler.HandleTask(readData, (dataEntry, isSuccessful) =>
            {
                if(!isSuccessful)
                {
                    onComplete?.Invoke(false);
                    return;
                }

                Task set = context.Set(userId, new(newPasswordHash, dataEntry.data));
                asyncHandler.HandleTask(set, onComplete);
            });
        }

        public void TryUpdateUserData(string userId, string password, UserData userData, Action<bool> onComplete)
        {
            if (!ProperlyInitialized(onComplete)) return;

            if (!Validate(userId, password))
            {
                onComplete?.Invoke(false);
                return;
            }

            string passwordHash = trackedUsers[userId];
            byte[] data = UserData.Serialize(userData);

            Task set = context.Set(userId, new(passwordHash, data));

            asyncHandler.HandleTask(set, onComplete);
        }

        public void GetUserData(string userId, Action<UserData, bool> onComplete)
        {
            if (!CheckInitialization(onComplete)) return;

            if (cachedUserDatas.TryGetValue(userId, out var data))
            {
                onComplete?.Invoke(data, true);
                return;
            }

            var readData = context.ReadData(userId);

            asyncHandler.HandleTask(readData, (entry, isSuccessful) =>
            {
                UserData userData = UserData.Deserialize(entry.data,
                    databaseManager.TowerTokenLibrary);
                cachedUserDatas[userId] = userData;
                onComplete?.Invoke(userData, true);
            });
        }

        public void TryDelete(string userId, Action<bool> onComplete)
        {
            if (!ProperlyInitialized(onComplete)) return;

            Task delete = context.Delete(userId);

            asyncHandler.HandleTask(delete, onComplete);
        }
    }
}