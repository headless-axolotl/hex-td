using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

using Lotl.Utility.Async;
using Lotl.Data.Towerset;
using Lotl.SceneManagement;
using Lotl.Generic.Events;

namespace Lotl.Data
{
    [RequireComponent(typeof(AsyncTaskProcessor))]
    public class DatabaseManager : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private TowerTokenLibrary towerTokenLibrary;
        [SerializeField] private SceneTransitionInitializer sceneTransitionInitializer;

        [Header("Settings")]
        [SerializeField] private LoadSettings loadSettings;

        [Header("Events")]
        [SerializeField] private GameEvent readyEvent;
        
        private AsyncTaskProcessor asyncProcessor;

        private DatabaseContext databaseContext = null;

        private int managersToInitialize = 0;
        private int initializedManagers = 0;
        private bool initializationIsSucessful = false;

        private UserContext userContext         = null;
        private TowersetContext towersetContext = null;
        private RunContext runContext           = null;

        private UserManager userManager         = null;
        private TowersetManager towersetManager = null;
        private RunManager runManager           = null;
        
        public TowerTokenLibrary TowerTokenLibrary => towerTokenLibrary;

        public UserManager UserManager => userManager;
        public TowersetManager TowersetManager => towersetManager;
        public RunManager RunManager => runManager;

        private void Start()
        {
            if(towerTokenLibrary == null)
            {
                Debug.LogError("Missing tower token library!");
                return;
            }

            if(sceneTransitionInitializer == null)
            {
                Debug.LogError("Missing SceneTransitionInitializer!");
                return;
            }

            asyncProcessor = GetComponent<AsyncTaskProcessor>();
            
            StartInitialization();
        }

        #region Initialization

        private void StartInitialization()
        {
            Debug.Log("Began initialization!");
            databaseContext = new();

            userContext     = new(databaseContext);
            towersetContext = new(databaseContext);
            runContext      = new(databaseContext);

            userManager     = new(userContext    , this, asyncProcessor);
            towersetManager = new(towersetContext, this, asyncProcessor);
            runManager      = new(runContext     , this, asyncProcessor);

            initializationIsSucessful = true;

            CreateTables();
        }

        private void CreateTables()
        {
            Task createTables = CreateTablesAsync();

            asyncProcessor.ProcessTask(createTables, OnTablesCreated, null);
        }

        private async Task CreateTablesAsync()
        {
            await userContext.CreateTableAsync();
            await towersetContext.CreateTableAsync();
            await runContext.CreateTableAsync();
        }

        private void OnTablesCreated(AsyncTaskResult result)
        {
            if(!result.WasSuccessful)
            {
                Debug.LogWarning(result.Message);
                ExitScene();
                return;
            }

            ContinueInitialization();
        }

        private void TallyManagersToBeInitialized()
        {
            if (loadSettings.Contains(LoadSettings.User))
                managersToInitialize++;
            if (loadSettings.Contains(LoadSettings.Towerset))
                managersToInitialize++;
            if (loadSettings.Contains(LoadSettings.Run))
                managersToInitialize++;
        }

        private void ContinueInitialization()
        {
            initializedManagers = 0;

            TallyManagersToBeInitialized();

            if(loadSettings.Contains(LoadSettings.User))
                userManager.Initialize(OnManagerInitialized);
            
            if(loadSettings.Contains(LoadSettings.Towerset))
                towersetManager.Initialize(OnManagerInitialized);

            if(loadSettings.Contains(LoadSettings.Run))
                runManager.Initialize(OnManagerInitialized);
        }

        private void OnManagerInitialized(AsyncTaskResult result)
        {
            if (!result.WasSuccessful)
            {
                initializationIsSucessful = false;
                Debug.LogWarning(result.Message);
            }

            initializedManagers++;

            if (initializedManagers == managersToInitialize)
            {
                CompleteInitialization();
            }
        }

        private void CompleteInitialization()
        {
            if(!initializationIsSucessful)
            {
                ExitScene();
                return;
            }

            Debug.Log("Initialization was successful!");
            readyEvent.Raise();
        }

        #endregion

        private void ExitScene()
        {
            Debug.LogError("Aborting operation!");
            // sceneTransitionInitializer.ReturnToPreviousScene();
        }

        private void OnDestroy()
        {
            databaseContext?.Dispose();
        }
    }
}