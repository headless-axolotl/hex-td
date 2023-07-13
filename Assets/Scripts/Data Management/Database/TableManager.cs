using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Data.SQLite;

namespace Lotl.DataManagement
{
    public abstract class TableManager : ScriptableObject
    {
        public abstract void CreateTable(SQLiteConnection connection);
    }
}
