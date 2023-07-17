using System.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SQLDatabase.Net.SQLDatabaseClient;

namespace Lotl.Data
{
    [CreateAssetMenu(fileName = "RunTable", menuName = "Lotl/Data/Tables/Run Table")]
    public class RunTable : TableManager
    {
        #region Constants

        private const string RunsTable = "run_table";
        private const string Id = "run_id";
        private const string Data = "run_data";

        #endregion

        public override void CreateTable()
        {
            using SqlDatabaseConnection connection = new(DatabaseSettings.ConnectionString);
            connection.Open();
            
            using SqlDatabaseCommand command = connection.CreateCommand() as SqlDatabaseCommand;
            command.CommandText = $"create table if not exists {RunsTable}({Id} text not null, {Data} blob not null, primary key({Id}) on conflict replace);";
            command.ExecuteNonQuery();

            connection.Close();
        }

        public void Set(string runId, byte[] data)
        {
            using SqlDatabaseConnection connection = new(DatabaseSettings.ConnectionString);
            connection.Open();

            using SqlDatabaseCommand command = connection.CreateCommand() as SqlDatabaseCommand;
            command.CommandText = $"insert into {RunsTable} values(@{Id}, @{Data});";
            command.Parameters.Add($"@{Id}", DbType.String).Value = runId;
            command.Parameters.Add($"@{Data}", DbType.Binary).Value = data;

            command.ExecuteNonQuery();

            connection.Close();
        }

        public byte[] ReadData(string runId)
        {
            using SqlDatabaseConnection connection = new(DatabaseSettings.ConnectionString);
            connection.Open();

            using SqlDatabaseCommand command = connection.CreateCommand() as SqlDatabaseCommand;
            command.CommandText = $"select {Data} from {RunsTable} where {Id} = @{Id};";
            command.Parameters.Add($"@{Id}", DbType.String).Value = runId;

            byte[] data = command.ExecuteScalar() as byte[];

            connection.Close();

            return data;
        }

        public List<string> ReadAll()
        {
            using SqlDatabaseConnection connection = new(DatabaseSettings.ConnectionString);
            connection.Open();

            using SqlDatabaseCommand command = connection.CreateCommand() as SqlDatabaseCommand;
            command.CommandText = $"select {Id} from {RunsTable};";

            using SqlDatabaseDataReader reader = command.ExecuteReader();
            List<string> result = new();
            while (reader.Read()) result.Add(reader.GetString(0));

            connection.Close();

            return result;
        }

        public void Delete(string runId)
        {
            using SqlDatabaseConnection connection = new(DatabaseSettings.ConnectionString);
            connection.Open();

            using SqlDatabaseCommand command = connection.CreateCommand() as SqlDatabaseCommand;
            command.CommandText = $"delete from {RunsTable} where {Id} = @{Id};";
            command.Parameters.Add($"@{Id}", DbType.String).Value = runId;

            command.ExecuteNonQuery();

            connection.Close();
        }
    }
}
