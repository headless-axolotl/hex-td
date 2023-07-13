using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;
using System.Data.SQLite;

namespace Lotl.DataManagement
{
    [CreateAssetMenu(fileName = "DatabaseManager", menuName = "Lotl/Data/Database Manager")]
    public class DatabaseManager : ScriptableObject
    {
        #region Constants

        private const string SAVE_FILE = "storage.data";
        private readonly string FILE_PATH = Application.persistentDataPath + SAVE_FILE;
        private readonly string CONNECTION_STRING
            = "URI=file:" + Application.persistentDataPath + SAVE_FILE;

        #endregion

        #region Properties

        private bool isInitialized = false;

        [SerializeField] private List<TableManager> tableManagers = new();
        private Dictionary<Type, TableManager> hashedTables = new();
        private SQLiteConnection connection = null;

        public IReadOnlyDictionary<Type, TableManager> Tables => hashedTables;

        #endregion

        #region Methods

        public void Initialize()
        {
            if (isInitialized) return;

            isInitialized = true;
            InitializeDatabase();
            HashTableManagers();

            Application.quitting += () => {
                connection?.Dispose();
            };
        }
        private void InitializeDatabase()
        {
            if (File.Exists(FILE_PATH))
            {
                EstablishConnection();
                return;
            }

            SQLiteConnection.CreateFile(FILE_PATH);
            
            EstablishConnection();

            foreach(TableManager tableManager in tableManagers)
                tableManager.CreateTable(connection);
        }
        private void EstablishConnection()
        {
            connection?.Dispose();
            connection = new SQLiteConnection(CONNECTION_STRING);
        }
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

