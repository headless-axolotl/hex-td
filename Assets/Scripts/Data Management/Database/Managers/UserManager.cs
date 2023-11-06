using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.Data
{
    public class UserManager
    {
        private readonly UserContext context;
        private readonly DatabaseManager databaseManager;
        private readonly AsyncTaskHandler asyncHandler;

        private HashSet<string> trackedUsers;
        // private Dictionary<string, userd>
    }
}