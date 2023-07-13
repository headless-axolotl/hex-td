using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.StateMachine;
using Lotl.Generic.Variables;
using Lotl.DataManagement;

namespace Lotl.Gameplay
{
    [RequireComponent(typeof(TowerBuilder))]
    public class GameManager : Driver
    {
        [SerializeField] private IntVariable resourceCount;
        [SerializeField] private RunDataManager runData;

        [SerializeField] private TowerBuilder towerBuilder;
        // Resource manager

        public int ResourceCount => resourceCount.Value;
        public RunDataManager RunData => runData;

        public bool ReadyFlag { get; set; }

        protected override void Awake()
        {
            resourceCount.Value = runData.CurrentSnapshot.ResourceAmount;
            towerBuilder = GetComponent<TowerBuilder>();

            base.Awake();
        }
    }
}
