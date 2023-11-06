using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using UnityEngine;
using SQLDatabase.Net.SQLDatabaseClient;

namespace Lotl.Data
{
    public class RunContext :
        IContext<byte[],  RunContext.Identity, RunContext.Identity>
    {
        #region Constants

        private string Table     => DatabaseSettings.RunTable;
        private string Name      => DatabaseSettings.RunName;
        private string RunUserId => DatabaseSettings.RunUserId;
        private string Data      => DatabaseSettings.RunData;

        #endregion

        public async Task CreateTable()
        {
            using SqlDatabaseConnection connection = new(DatabaseSettings.ConnectionString);
            await connection.OpenAsync();

            using SqlDatabaseCommand command = connection.CreateCommand() as SqlDatabaseCommand;
            command.CommandText =
            $@"create table if not exists {Table}(
                {Name} text not null,
                {RunUserId} text not null,
                {Data} blob not null,
                primary key ({Name}, {RunUserId}) on conflict replace,
                foreign key ({RunUserId}) references {DatabaseSettings.UserTable} ({DatabaseSettings.UserId})
                on delete cascade on update no action
            );";
            await command.ExecuteNonQueryAsync();

            await connection.CloseAsync();
        }

        public async Task Set(Identity key, byte[] data)
        {
            using SqlDatabaseConnection connection = new(DatabaseSettings.ConnectionString);
            await connection.OpenAsync();

            using SqlDatabaseCommand command = connection.CreateCommand() as SqlDatabaseCommand;
            command.CommandText = $@"insert into {Table} values(
                @{Name}, @{RunUserId}, @{Data});";

            command.Parameters.Add($"@{Name}",      DbType.String).Value = key.name;
            command.Parameters.Add($"@{RunUserId}", DbType.String).Value = key.userId;
            command.Parameters.Add($"@{Data}",      DbType.Binary).Value = data;

            await command.ExecuteNonQueryAsync();

            await connection.CloseAsync();
        }

        public async Task<byte[]> ReadData(Identity key)
        {
            using SqlDatabaseConnection connection = new(DatabaseSettings.ConnectionString);
            await connection.OpenAsync();

            using SqlDatabaseCommand command = connection.CreateCommand() as SqlDatabaseCommand;
            command.CommandText = $@"select {Data} from {Table} where {Name} = @{Name} and {RunUserId} = @{RunUserId};";
            command.Parameters.Add($"@{Name}",      DbType.String).Value = key.name;
            command.Parameters.Add($"@{RunUserId}", DbType.String).Value = key.userId;

            using SqlDatabaseDataReader reader = await command.ExecuteReaderAsync() as SqlDatabaseDataReader;
            await reader.ReadAsync();

            byte[] data =   await reader.GetFieldValueAsync<byte[]>(Data);
            
            await reader.CloseAsync();
            await connection.CloseAsync();

            return data;
        }

        public async Task<List<Identity>> ReadAll()
        {
            using SqlDatabaseConnection connection = new(DatabaseSettings.ConnectionString);
            await connection.OpenAsync();

            using SqlDatabaseCommand command = connection.CreateCommand() as SqlDatabaseCommand;
            command.CommandText = $@"select {Name}, {RunUserId} from {Table};";
            
            using SqlDatabaseDataReader reader = await command.ExecuteReaderAsync() as SqlDatabaseDataReader;

            List<Identity> descriptiveEntries = new();

            while (await reader.ReadAsync())
            {
                string name   = await reader.GetFieldValueAsync<string>(Name);
                string userId = await reader.GetFieldValueAsync<string>(RunUserId);

                Identity currentDescriptiveEntry = new(name, userId);

                descriptiveEntries.Add(currentDescriptiveEntry);
            }

            await reader.CloseAsync();
            await connection.CloseAsync();

            return descriptiveEntries;
        }

        public async Task Delete(Identity key)
        {
            using SqlDatabaseConnection connection = new(DatabaseSettings.ConnectionString);
            await connection.OpenAsync();

            using SqlDatabaseCommand command = connection.CreateCommand() as SqlDatabaseCommand;
            command.CommandText = $@"delete from {Table} where {Name} = @{Name} and {RunUserId} = @{RunUserId};";
            command.Parameters.Add($"@{Name}",      DbType.String).Value = key.name;
            command.Parameters.Add($"@{RunUserId}", DbType.String).Value = key.userId;

            await command.ExecuteNonQueryAsync();

            await connection.CloseAsync();
        }

        public struct Identity : IEquatable<Identity>
        {
            public string name;
            public string userId;

            public Identity(string name, string userId)
            {
                this.name = name;
                this.userId = userId;
            }

            public bool Equals(Identity other)
            {
                return name == other.name && userId == other.userId;
            }
        }
    }
}
