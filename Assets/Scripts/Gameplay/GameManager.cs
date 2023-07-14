using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.StateMachine;
using Lotl.DataManagement;

namespace Lotl.Gameplay
{
    [RequireComponent(typeof(TowerBuilder))]
    public class GameManager : Driver
    {
        #region Properties

        [Header("Data")]
        [SerializeField] private RunState runState = new();
        [SerializeField] private RunDataManager runData;
        [SerializeField] private RunTableManager runTableManager;
        [SerializeField] private DatabaseManager databaseManager;

        [Header("Runtime")]
        [SerializeField] private TowerBuilder towerBuilder;

        public RunState RunState => runState;
        public RunDataManager RunData => runData;
        public RunTableManager RunTableManager => runTableManager;
        public DatabaseManager DatabaseManager => databaseManager;

        [SerializeField] private bool readyFlag;
        public bool ReadyFlag { get => readyFlag; set => readyFlag = value; }

        #endregion

        #region Methods

        /// <summary>
        /// Loads the state of the game from the referenced RunDataManager before the state machine
        /// has the chance to override it.
        /// </summary>
        protected override void Awake()
        {
            towerBuilder = GetComponent<TowerBuilder>();
            LoadState();

            base.Awake();
        }

        public void LoadState()
        {
            runState.Load(runData.RunInfo, towerBuilder);
        }

        public void SaveState()
        {
            runState.Save(runData.RunInfo);
            // TODO: runTableManager.Save(runState.runId, runState.Serialize());
        }

        #endregion
    }
}
