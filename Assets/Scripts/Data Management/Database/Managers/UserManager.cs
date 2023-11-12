using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

using Lotl.Utility.Async;
using Lotl.Data.Users;
using BCrypter = BCrypt.Net.BCrypt;

namespace Lotl.Data
{
    using Result = AsyncTaskResult;

    public class UserManager : BaseManager
    {
        #region Error Messages

        private const string UserAlreadyExists = "user already exists";

        private const string IncorrectOldPassword = "incorrect old password";

        #endregion

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

        public void Initialize(Action<Result> onComplete)
        {
            if (IsInitialized)
            {
                onComplete?.Invoke(Result.OK);
                return;
            }

            var readAll = context.ReadAll();

            asyncHandler.HandleTask(readAll, onComplete, onSuccess: (entries) =>
            {
                trackedUsers = new(entries.ToDictionary(
                    entry => entry.id,
                    entry => entry.passwordHash));

                Initialize();
            });
        }

        public bool Validate(string userId, string password)
        {
            if (!ProperlyInitialized(null)) return false;

            if (!trackedUsers.TryGetValue(userId, out var passwordHash))
                return false;

            return BCrypter.EnhancedVerify(password, passwordHash);
        }

        public void CreateUser(
            string userId,
            string password,
            UserData baseData,
            Action<Result> onComplete)
        {
            if (!ProperlyInitialized(onComplete)) return;

            if (trackedUsers.ContainsKey(userId))
            {
                onComplete?.Invoke(new Result(false, UserAlreadyExists));
                return;
            }

            string passwordHash = BCrypter.EnhancedHashPassword(password);
            byte[] data = UserData.Serialize(baseData);

            Task set = context.Set(userId, new(passwordHash, data));

            asyncHandler.HandleTask(set, onComplete, onSuccess: () =>
            {
                trackedUsers[userId] = passwordHash;
                cachedUserDatas[userId] = baseData;
            });
        }

        public void UpdatePassword(
            string userId,
            string oldPassword,
            string newPassword,
            Action<Result> onComplete)
        {
            if (!ProperlyInitialized(onComplete)) return;

            if (!Validate(userId, oldPassword))
            {
                onComplete?.Invoke(new Result(false, IncorrectOldPassword));
                return;
            }

            string newPasswordHash = BCrypter.EnhancedHashPassword(newPassword);

            var readData = context.ReadData(userId);

            asyncHandler.HandleTask(readData, null, onSuccess: (dataEntry) =>
            {
                Task set = context.Set(userId, new(newPasswordHash, dataEntry.data));
                asyncHandler.HandleTask(set, onComplete, onSuccess: () =>
                {
                    trackedUsers[userId] = newPasswordHash;
                });
            });
        }

        public void UpdateUserData(
            string userId,
            string password,
            UserData userData,
            Action<Result> onComplete)
        {
            if (!ProperlyInitialized(onComplete)) return;

            if (!Validate(userId, password))
            {
                onComplete?.Invoke(new Result(false, incorrec));
                return;
            }

            string passwordHash = trackedUsers[userId];
            byte[] data = UserData.Serialize(userData);

            Task set = context.Set(userId, new(passwordHash, data));

            asyncHandler.HandleTask(set, onComplete, onSuccess: () =>
            {
                cachedUserDatas[userId] = userData;
            });
        }

        public void GetUserData(
            string userId,
            Action<Result, UserData> onComplete)
        {
            if (!ProperlyInitialized(onComplete)) return;

            if (cachedUserDatas.TryGetValue(userId, out var data))
            {
                onComplete?.Invoke(Result.OK, data);
                return;
            }

            var readData = context.ReadData(userId);

            asyncHandler.HandleTask(readData, onComplete, onSuccess: (entry) =>
            {
                UserData userData = UserData.Deserialize(entry.data,
                    databaseManager.TowerTokenLibrary);
                
                cachedUserDatas[userId] = userData;
                
                return userData;
            });
        }

        public void Delete(string userId, Action<Result> onComplete)
        {
            if (!ProperlyInitialized(onComplete)) return;

            Task delete = context.Delete(userId);

            asyncHandler.HandleTask(delete, onComplete, onSuccess: () =>
            {
                trackedUsers.Remove(userId);
                cachedUserDatas.Remove(userId);
            });
        }
    }
}