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
    }
}

