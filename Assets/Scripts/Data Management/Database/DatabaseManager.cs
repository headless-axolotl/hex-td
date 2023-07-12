using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;

namespace Lotl.DataManagement
{
    [CreateAssetMenu(fileName = "DatabaseManager", menuName = "Database/Manager")]
    public class DatabaseManager : ScriptableObject
    {
        private string path => "URI=file:" + Application.persistentDataPath + SAVE_FILE_PATH;

        public void InitializeDatabase()
        {
            Directory.CreateDirectory(Application.persistentDataPath + DIRECTORY_PATH);
        }

        // create tables

        // make new run entry

        // read new run entry

        // check if a run entry exists

        // update run entry

        private const string DIRECTORY_PATH = "/Data";
        private const string SAVE_FILE_PATH = "/Data/storage.data";
    }
}

