using SQLDatabase.Net.SQLDatabaseClient;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using UnityEngine;

namespace Lotl.Data
{
    public class UserContext
        : IContext<UserContext.DataEntry, UserContext.DescriptiveEntry, string>
    {
        #region Constants

        private string Table        => DatabaseSettings.UserTable;
        private string Id           => DatabaseSettings.UserId;
        private string PasswordHash => DatabaseSettings.UserPasswordHash;
        private string Data         => DatabaseSettings.UserData;

        #endregion

        public async Task CreateTable()
        {
            using SqlDatabaseConnection connection = new(DatabaseSettings.ConnectionString);
            await connection.OpenAsync();

            using SqlDatabaseCommand command = connection.CreateCommand() as SqlDatabaseCommand;
            command.CommandText =
            $@"create table if not exists {Table}(
                {Id} text not null,
                {PasswordHash} text not null,
                {Data} blob not null,
                primary key({Id}) on conflict replace,
            );";

            await command.ExecuteNonQueryAsync();

            await connection.CloseAsync();
        }

        public async Task Set(string key, DataEntry data)
        {
            using SqlDatabaseConnection connection = new(DatabaseSettings.ConnectionString);
            await connection.OpenAsync();

            using SqlDatabaseCommand command = connection.CreateCommand() as SqlDatabaseCommand;
            command.CommandText = $@"insert into {Table} values(
                @{Id}, @{PasswordHash}, @{Data});";

            command.Parameters.Add($"@{Id}",           DbType.String).Value = key;
            command.Parameters.Add($"@{PasswordHash}", DbType.String).Value = data.passwordHash;
            command.Parameters.Add($"@{Data}",         DbType.Binary).Value = data.data;

            await command.ExecuteNonQueryAsync();

            await connection.CloseAsync();
        }

        public async Task<DataEntry> ReadData(string key)
        {
            using SqlDatabaseConnection connection = new(DatabaseSettings.ConnectionString);
            await connection.OpenAsync();

            using SqlDatabaseCommand command = connection.CreateCommand() as SqlDatabaseCommand;
            command.CommandText = $@"select {Id}, {PasswordHash}, {Data}
                from {Table} where {Id} = @{Id};";

            command.Parameters.Add($"@{Id}", DbType.String).Value = key;

            using SqlDatabaseDataReader reader = await command.ExecuteReaderAsync() as SqlDatabaseDataReader;

            await reader.ReadAsync();

            string passwordHash = await reader.GetFieldValueAsync<string>(PasswordHash);
            byte[] data         = await reader.GetFieldValueAsync<byte[]>(Data);

            DataEntry dataEntry = new(passwordHash, data);

            await reader.CloseAsync();
            await connection.CloseAsync();

            return dataEntry;
        }

        public async Task<List<DescriptiveEntry>> ReadAll()
        {
            using SqlDatabaseConnection connection = new(DatabaseSettings.ConnectionString);
            await connection.OpenAsync();

            using SqlDatabaseCommand command = connection.CreateCommand() as SqlDatabaseCommand;
            command.CommandText = $@"select {Id}, {PasswordHash} from {Table};";

            using SqlDatabaseDataReader reader = await command.ExecuteReaderAsync() as SqlDatabaseDataReader;

            List<DescriptiveEntry> descriptiveEntries = new();
            while (await reader.ReadAsync())
            {
                string id           = await reader.GetFieldValueAsync<string>(Id);
                string passwordHash = await reader.GetFieldValueAsync<string>(PasswordHash);

                DescriptiveEntry currentDescriptiveEntry = new(id, passwordHash);

                descriptiveEntries.Add(currentDescriptiveEntry);
            }

            await reader.CloseAsync();
            await connection.CloseAsync();

            return descriptiveEntries;
        }

        public async Task Delete(string key)
        {
            using SqlDatabaseConnection connection = new(DatabaseSettings.ConnectionString);
            await connection.OpenAsync();

            using SqlDatabaseCommand command = connection.CreateCommand() as SqlDatabaseCommand;
            command.CommandText = $"delete from {Table} where {Id} = @{Id};";
            command.Parameters.Add($"@{Id}", DbType.Int32).Value = key;

            await command.ExecuteNonQueryAsync();

            await connection.CloseAsync();
        }

        public struct DataEntry
        {
            public string passwordHash;
            public byte[] data;

            public DataEntry(string passwordHash, byte[] data)
            {
                this.passwordHash = passwordHash;
                this.data = data;
            }
        }

        public struct DescriptiveEntry
        {
            public string id;
            public string passwordHash;

            public DescriptiveEntry(string id, string passwordHash)
            {
                this.id = id;
                this.passwordHash = passwordHash;
            }
        }
    }
}