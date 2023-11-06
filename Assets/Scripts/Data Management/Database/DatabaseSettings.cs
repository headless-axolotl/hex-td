using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.Data
{
    public class DatabaseSettings
    {
        public static string Schema => "Hexdefence";
        public static string FileName => "storage.data";
        public static string FilePath => Application.persistentDataPath + "/" + FileName;
        public static string ConnectionString => $"SchemaName={Schema};URI=file://{FilePath}";

        #region User Table

        public const string UserTable        = "user_table";
        public const string UserId           = "user_id";
        public const string UserPasswordHash = "user_password_hash";
        public const string UserData         = "user_data";

        #endregion

        #region Towerset Table

        public const string TowersetTable    = "towerset_table";
        public const string TowersetName     = "towerset_name";
        public const string TowersetUserId   = "towerset_user_id";
        public const string TowersetValidity = "towerset_validity";
        public const string TowersetData     = "towerset_data";

        #endregion

        #region Run Table

        public const string RunTable  = "run_table";
        public const string RunName   = "run_name";
        public const string RunUserId = "run_user_id";
        public const string RunData   = "run_data";

        #endregion
    }
}

