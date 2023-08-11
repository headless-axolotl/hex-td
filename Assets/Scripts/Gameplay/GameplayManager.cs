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
        [SerializeField] private RunDataObject runDataObject;
        [SerializeField] private RunTable runTableManager;

        [Header("Runtime")]
        [SerializeField] private TowerBuilder towerBuilder;

        public RunState RunState => runState;
        public RunDataObject RunDataObject => runDataObject;
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
            runState.Load(runDataObject.Data.RunInfo, towerBuilder);
        }

        public void SaveState()
        {
            runState.Save(runDataObject.Data.RunInfo);
            runTableManager.Set(
                runDataObject.Data.RunId,
                RunData.Serialize(runDataObject.Data));
        }

        #endregion
    }
}
