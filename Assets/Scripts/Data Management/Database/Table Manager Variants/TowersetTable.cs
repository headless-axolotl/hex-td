using System.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SQLDatabase.Net.SQLDatabaseClient;

namespace Lotl.Data
{
    [CreateAssetMenu(fileName = "TowersetTable", menuName = "Lotl/Data/Tables/Towerset Table")]
    public class TowersetTable : TableManager
    {
        #region Constants

        private const string TowersetsTable = "towerset_table";
        private const string Id = "towerset_id";
        private const string Validity = "towerset_validity";
        private const string Data = "towerset_data";

        #endregion

        public override void CreateTable()
        {
            using SqlDatabaseConnection connection = new(DatabaseSettings.ConnectionString);
            connection.Open();

            using SqlDatabaseCommand command = connection.CreateCommand() as SqlDatabaseCommand;
            command.CommandText = $"create table if not exists {TowersetsTable}({Id} text not null, {Validity} integer not null, {Data} blob not null, primary key({Id}) on conflict replace)";
            command.ExecuteNonQuery();

            connection.Close();
        }

        public void Set(string towersetId, bool validity, byte[] data)
        {
            using SqlDatabaseConnection connection = new(DatabaseSettings.ConnectionString);
            connection.Open();

            using SqlDatabaseCommand command = connection.CreateCommand() as SqlDatabaseCommand;
            command.CommandText = $"insert into {TowersetsTable} values(@{Id}, @{Validity}, @{Data})";
            command.Parameters.Add($"@{Id}", DbType.String).Value = towersetId;
            command.Parameters.Add($"@{Validity}", DbType.Int32).Value = (validity ? 1 : 0);
            command.Parameters.Add($"@{Data}", DbType.Binary).Value = data;

            command.ExecuteNonQuery();

            connection.Close();
        }

        public byte[] ReadData(string towersetId)
        {
            using SqlDatabaseConnection connection = new(DatabaseSettings.ConnectionString);
            connection.Open();

            using SqlDatabaseCommand command = connection.CreateCommand() as SqlDatabaseCommand;
            command.CommandText = $"select {Data} from {TowersetsTable} where {Id} = @{Id};";
            command.Parameters.Add($"@{Id}", DbType.String).Value = towersetId;

            byte[] data = command.ExecuteScalar() as byte[];

            connection.Close();
            
            return data;
        }

        public List<Entry> ReadAll()
        {
            using SqlDatabaseConnection connection = new(DatabaseSettings.ConnectionString);
            connection.Open();

            using SqlDatabaseCommand command = connection.CreateCommand() as SqlDatabaseCommand;
            command.CommandText = $"select {Id}, {Validity} from {TowersetsTable};";

            using SqlDatabaseDataReader reader = command.ExecuteReader();
            List<Entry> result = new();
            while (reader.Read())
            {
                result.Add(new(reader.GetString(0), reader.GetInt32(1) != 0));
            }

            connection.Close();

            return result;
        }

        public void Delete(string towersetId)
        {
            using SqlDatabaseConnection connection = new(DatabaseSettings.ConnectionString);
            connection.Open();

            using SqlDatabaseCommand command = connection.CreateCommand() as SqlDatabaseCommand;
            command.CommandText = $"delete from {TowersetsTable} where {Id} = @{Id};";
            command.Parameters.Add($"@{Id}", DbType.String).Value = towersetId;

            command.ExecuteNonQuery();

            connection.Close();
        }

        public struct Entry
        {
            public string id;
            public bool isValid;
            public Entry(string id, bool validity)
            {
                this.id = id;
                this.isValid = validity;
            }
        }
    }
}
