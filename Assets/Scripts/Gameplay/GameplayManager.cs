using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.StateMachine;
using Lotl.Data;
using Lotl.Data.Runs;
using Lotl.Data.Users;

namespace Lotl.Gameplay
{
    [RequireComponent(typeof(TowerBuilder))]
    public class GameplayManager : Driver
    {
        #region Properties

        [Header("Data")]
        [SerializeField] private DatabaseManager databaseManager;
        [SerializeField] private UserCookie userCookie;
        [SerializeField] private RunState runState;
        [SerializeField] private RunDataObject crossSceneData;

        [Header("Runtime")]
        [SerializeField] private TowerBuilder towerBuilder;

        public RunState RunState => runState;

        [SerializeField] private bool readyFlag;
        public bool ReadyFlag { get => readyFlag; set => readyFlag = value; }

        #endregion

        #region Methods

        protected override void Awake()
        {
            if (databaseManager == null)
            {
                Debug.LogError("Missing database manager!");
            }

            towerBuilder = GetComponent<TowerBuilder>();
            LoadState();

            base.Awake();
        }

        public void TriggerReadyFlag()
        {
            ReadyFlag = true;
        }

        public void LoadState()
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

        #endregion
    }
}
