using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.StateMachine;
using Lotl.Data;
using Lotl.Data.Runs;
using Lotl.Generic.Variables;

namespace Lotl.Gameplay
{
    [RequireComponent(typeof(TowerBuilder))]
    public class GameplayManager : Driver
    {
        #region Properties

        [Header("Data")]
        [SerializeField] private RunState runState = new();
        [SerializeField] private RunDataObject runData;
        [SerializeField] private RunTable runTableManager;

        [Header("Runtime")]
        [SerializeField] private TowerBuilder towerBuilder;

        public RunState RunState => runState;
        public RunDataObject RunData => runData;
        public RunTable RunTableManager => runTableManager;

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
            runTableManager.Set(
                runData.RunId,
                runData.Serialize());
        }

        #endregion
    }
}
