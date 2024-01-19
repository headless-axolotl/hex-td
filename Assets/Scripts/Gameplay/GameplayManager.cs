using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Data;
using Lotl.Data.Runs;
using Lotl.Data.Users;
using Lotl.Gameplay.Waves;

namespace Lotl.Gameplay
{
    using Result = Utility.Async.AsyncTaskResult;

    [RequireComponent(typeof(TowerBuilder))]
    public class GameplayManager : MonoBehaviour
    {
        #region Properties

        [Header("Data")]
        [SerializeField] private DatabaseManager databaseManager;
        [SerializeField] private UserCookie userCookie;
        [SerializeField] private RunState runState;
        [SerializeField] private RunDataObject crossSceneData;

        [Header("Runtime")]
        [SerializeField] private TowerBuilder towerBuilder;
        [SerializeField] private WaveSummoner waveSummoner;

        public RunState RunState => runState;

        #endregion

        protected void Awake()
        {
            if (databaseManager == null)
            {
                Debug.LogError("Missing database manager!");
            }

            towerBuilder = GetComponent<TowerBuilder>();
            LoadState();
        }

        private void LoadState()
        {
            runState.Load(crossSceneData.Data.RunInfo, towerBuilder);
        }

        public void SaveState()
        {
            runState.Save(crossSceneData.Data.RunInfo);

            RunContext.Identity runIdentity = new(crossSceneData.Data.RunId, userCookie.UserId);

            databaseManager.RunManager.Set(runIdentity, crossSceneData.Data,
            onCompleted: (result) =>
            {
                if(!result.WasSuccessful)
                {
                    Debug.LogWarning(result.Message);
                    #warning Some kind of warning that save was not successful
                }
            });
        }

        public void BeginWave()
        {
            SaveState();
            waveSummoner.StartWave();
        }

        public void OnEndWave()
        {
            SaveState();
        }

        public void EndRun()
        {
            AddRewardToUser(onCompleted: (result) =>
            {
                if(!result.WasSuccessful)
                {
                    Debug.LogWarning(result.Message);
                    return;
                }

                DeleteEndedRun();

                #warning Do end run sequence!
            });
        }

        private void AddRewardToUser(Action<Result> onCompleted)
        {
            int reward = crossSceneData.Data.RunInfo.WaveIndex;
            databaseManager.UserManager.GetUserData(userCookie.UserId,
            onCompleted: (result, data) =>
            {
                if(!result.WasSuccessful)
                {
                    onCompleted.Invoke(result);
                    return;
                }

                data.MetaCurrencyAmount += reward;

                UpdateUserData(data, onCompleted);
            });
        }

        private void UpdateUserData(UserData userData, Action<Result> onCompleted)
        {
            databaseManager.UserManager.UpdateUserData(
                userCookie.UserId,
                userCookie.Password,
                userData,
                onCompleted);
        }

        private void DeleteEndedRun()
        {
            RunContext.Identity runIdentity = new(crossSceneData.Data.RunId, userCookie.UserId);
            databaseManager.RunManager.Delete(runIdentity, null);
        }
    }
}
