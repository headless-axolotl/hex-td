using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System;

namespace Lotl.DBManagement
{
    [CreateAssetMenu(fileName = "DatabaseManager", menuName = "Database/Manager")]
    public class DatabaseManager : ScriptableObject
    {
        
        private string path => "URI=file:" + Application.persistentDataPath + SAVE_FILE_PATH;

        public void InitializeDatabase()
        {
            Directory.CreateDirectory(Application.persistentDataPath + DIRECTORY_PATH);

        }

        private const string DIRECTORY_PATH = "/Data";
        private const string SAVE_FILE_PATH = "/Data/storage.data";
    }
}

