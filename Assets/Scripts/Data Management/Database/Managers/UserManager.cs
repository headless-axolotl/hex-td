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
        private readonly AsyncTaskProcessor asyncProcessor;

        private SortedDictionary<string, string> trackedUsers;
        private Dictionary<string, UserData> cachedUserDatas;

        public IReadOnlyDictionary<string, string> TrackedUsers => trackedUsers;

        public UserManager(
            UserContext context,
            DatabaseManager databaseManager,
            AsyncTaskProcessor asyncHandler)
        {
            this.context = context;
            this.databaseManager = databaseManager;
            this.asyncProcessor = asyncHandler;

            trackedUsers = null;
            cachedUserDatas = new();
        }

        public void Initialize(Action<Result> onCompleted)
        {
            if (IsInitialized)
            {
                onCompleted?.Invoke(Result.OK);
                return;
            }

            var readAll = context.ReadAllAsync();

            asyncProcessor.ProcessTask(readAll, onCompleted, onSuccess: (entries) =>
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
            Action<Result> onCompleted)
        {
            if (!ProperlyInitialized(onCompleted)) return;

            if (trackedUsers.ContainsKey(userId))
            {
                onCompleted?.Invoke(new Result(false, UserAlreadyExists));
                return;
            }

            string passwordHash = BCrypter.EnhancedHashPassword(password);
            byte[] data = UserData.Serialize(baseData);

            Task set = context.SetAsync(userId, new(passwordHash, data));

            asyncProcessor.ProcessTask(set, onCompleted, onSuccess: () =>
            {
                trackedUsers[userId] = passwordHash;
                cachedUserDatas[userId] = baseData;
            });
        }

        public void UpdatePassword(
            string userId,
            string oldPassword,
            string newPassword,
            Action<Result> onCompleted)
        {
            if (!ProperlyInitialized(onCompleted)) return;

            if (!Validate(userId, oldPassword))
            {
                onCompleted?.Invoke(new Result(false, IncorrectOldPassword));
                return;
            }

            string newPasswordHash = BCrypter.EnhancedHashPassword(newPassword);

            var readDataAndSetPassword = ReadDataAndSetPasswordAsync(
                userId, newPasswordHash);

            asyncProcessor.ProcessTask(readDataAndSetPassword, onCompleted,
            onSuccess: () =>
            {
                trackedUsers[userId] = newPasswordHash;
            });
        }

        private async Task ReadDataAndSetPasswordAsync(string userId, string newPasswordHash)
        {
            byte[] data = (await context.ReadDataAsync(userId)).data;

            await context.SetAsync(userId, new(newPasswordHash, data));
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
                onComplete?.Invoke(new Result(false, IncorrectOldPassword));
                return;
            }

            string passwordHash = trackedUsers[userId];
            byte[] data = UserData.Serialize(userData);

            Task set = context.SetAsync(userId, new(passwordHash, data));

            asyncProcessor.ProcessTask(set, onComplete, onSuccess: () =>
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

            var readData = context.ReadDataAsync(userId);

            asyncProcessor.ProcessTask(readData, onComplete, onSuccess: (entry) =>
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

            Task delete = context.DeleteAsync(userId);

            asyncProcessor.ProcessTask(delete, onComplete, onSuccess: () =>
            {
                trackedUsers.Remove(userId);
                cachedUserDatas.Remove(userId);
            });
        }
    }
}