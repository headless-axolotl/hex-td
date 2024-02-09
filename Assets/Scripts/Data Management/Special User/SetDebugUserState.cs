using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Data.Runs;
using Lotl.Data.Towerset;
using Lotl.Data.Users;
using Lotl.Generic.Variables;

namespace Lotl.Data
{
    public class SetDebugUserState : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private UserCookie userCookie;
        [SerializeField] private RunDataObject crossSceneData;
        [SerializeField] private IntVariable resources;
        [SerializeField] private TowerTokenLibrary towerTokenLibrary;
        [Header("Configuration")]
        [SerializeField] private StringReference debugUserName;

        private void Awake()
        {
            if (debugUserName != userCookie.UserId)
                return;
            ApplyTowerset();
        }

        private void Update()
        {
            if (debugUserName != userCookie.UserId)
                return;
            SetResources();
        }

        private void ApplyTowerset()
        {
            crossSceneData.Data.TowersetInfo = new TowersetInfo(towerTokenLibrary.TowerTokens);
        }

        private void SetResources()
        {
            if (resources.Value != ConstantResources)
                resources.Value = ConstantResources;
        }

        private const int ConstantResources = 1024;
    }
}
