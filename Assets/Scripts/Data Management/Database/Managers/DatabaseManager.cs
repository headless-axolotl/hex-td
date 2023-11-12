using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Utility.Async;
using Lotl.Data.Towerset;

namespace Lotl.Data
{
    [RequireComponent(typeof(AsyncTaskHandler))]
    public class DatabaseManager : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private TowerTokenLibrary towerTokenLibrary;

        public TowerTokenLibrary TowerTokenLibrary => towerTokenLibrary;

        private AsyncTaskHandler asyncHandler;

        private TowersetManager towersetManager;
        private RunManager runManager;
        private UserManager userManager;

        private void Awake()
        {
            towersetManager = new(new(), this, asyncHandler);
            runManager      = new(new(), this, asyncHandler);
            userManager     = new(new(), this, asyncHandler);
        }

        // Coroutine that checks if the initialization was successful
        // (try a couple of times? then crash? the program can't debug itself)
    }

}