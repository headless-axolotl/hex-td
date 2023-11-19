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
        [SerializeField] private RunState runState;
        [SerializeField] private RunDataObject crossSceneData;
#warning FIX THIS!
        // [SerializeField] private RunTable runTableManager;

        [Header("Runtime")]
        [SerializeField] private TowerBuilder towerBuilder;

        public RunState RunState => runState;
        // public RunTable RunTableManager => runTableManager;

        [SerializeField] private bool readyFlag;
        public bool ReadyFlag { get => readyFlag; set => readyFlag = value; }

        #endregion

        #region Methods

        protected override void Awake()
        {
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
#warning HERE TOO!!!
            //runTableManager.Set(
            //    crossSceneData.Data.RunId,
            //    RunData.Serialize(crossSceneData.Data));
        }

        #endregion
    }
}
