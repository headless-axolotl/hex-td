using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;
using SQLDatabase.Net;

namespace Lotl.DataManagement
{
    [CreateAssetMenu(fileName = "DatabaseManager", menuName = "Lotl/Data/Database Manager")]
    public class DatabaseManager : ScriptableObject
    {
        #region Constants

        private const string SCHEMA = "Hexdefence";
        private const string SAVE_FILE = "storage.data";
        private string FILE_PATH => Application.persistentDataPath + "/" + SAVE_FILE;
        private string CONNECTION_STRING => "URI=file:" + Application.persistentDataPath + "/" + SAVE_FILE;

        #endregion

        #region Properties

        [SerializeField] private List<TableManager> tableManagers = new();
        private Dictionary<Type, TableManager> hashedTables = new();
        
        public IReadOnlyDictionary<Type, TableManager> Tables => hashedTables;

        #endregion

        #region Methods

        private void HashTableManagers()
        {
            foreach (TableManager tableManager in tableManagers)
            {
                Type tableType = tableManagers.GetType();
                if (hashedTables.ContainsKey(tableType))
                {
                    Debug.LogWarning("Skipping duplicate TableManager type in " +
                        "tableManagers list of DatabaseManager!");
                    continue;
                }
                hashedTables.Add(tableType, tableManager);
            }
        }

        #endregion
    }
}

