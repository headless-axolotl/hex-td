using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLDatabase.Net.SQLDatabaseClient;
using UnityEngine;
using System;

namespace Lotl.Data
{
    public class TowersetContext :
        IContext<TowersetContext.DataEntry, TowersetContext.DescriptiveEntry, TowersetContext.Identity>
    {
        #region Constants

        private string Table          => DatabaseSettings.TowersetTable;
        private string Name           => DatabaseSettings.TowersetName;
        private string TowersetUserId => DatabaseSettings.TowersetUserId;
        private string Validity       => DatabaseSettings.TowersetValidity;
        private string Data           => DatabaseSettings.TowersetData;

        #endregion

        public async Task CreateTable()
        {
            using SqlDatabaseConnection connection = new(DatabaseSettings.ConnectionString);
            await connection.OpenAsync();

            using SqlDatabaseCommand command = connection.CreateCommand() as SqlDatabaseCommand;
            command.CommandText =
            $@"create table if not exists {Table}(
                {Name} text not null,
                {TowersetUserId} text not null,
                {Validity} integer not null,
                {Data} blob not null,
                primary key({Name}, {TowersetUserId}) autoincrement on conflict replace,
                foreign key({TowersetUserId}) references {DatabaseSettings.UserTable} ({DatabaseSettings.UserId})
                on delete cascade on update no action
            );";
            await command.ExecuteNonQueryAsync();

            await connection.CloseAsync();
        }

        public async Task Set(Identity key, DataEntry data)
        {
            using SqlDatabaseConnection connection = new(DatabaseSettings.ConnectionString);
            await connection.OpenAsync();

            using SqlDatabaseCommand command = connection.CreateCommand() as SqlDatabaseCommand;
            command.CommandText = $@"insert into {Table} values(
                @{Name}, @{TowersetUserId}, @{Validity}, @{Data});";
            
            command.Parameters.Add($"@{Name}",           DbType.String).Value = key.name;
            command.Parameters.Add($"@{TowersetUserId}", DbType.String).Value = key.userId;
            command.Parameters.Add($"@{Validity}",       DbType.Int32 ).Value = data.isValid ? 1 : 0;
            command.Parameters.Add($"@{Data}",           DbType.Binary).Value = data.data;

            await command.ExecuteNonQueryAsync();

            await connection.CloseAsync();
        }

        public async Task<DataEntry> ReadData(Identity key)
        {
            using SqlDatabaseConnection connection = new(DatabaseSettings.ConnectionString);
            await connection.OpenAsync();

            using SqlDatabaseCommand command = connection.CreateCommand() as SqlDatabaseCommand;
            command.CommandText = $@"select {Name}, {TowersetUserId}, {Validity}, {Data}
                from {Table} where {Name} = @{Name} and {TowersetUserId} = @{TowersetUserId};";            
            command.Parameters.Add($"@{Name}",           DbType.String).Value = key.name;
            command.Parameters.Add($"@{TowersetUserId}", DbType.String).Value = key.userId;

            using SqlDatabaseDataReader reader = await command.ExecuteReaderAsync() as SqlDatabaseDataReader;

            await reader.ReadAsync();

            bool isValid  = await reader.GetFieldValueAsync<int>(Validity) != 0;
            byte[] data   = await reader.GetFieldValueAsync<byte[]>(Data);

            DataEntry dataEntry = new(isValid, data);
            
            await reader.CloseAsync();
            await connection.CloseAsync();

            return dataEntry;
        }

        public async Task<List<DescriptiveEntry>> ReadAll()
        {
            using SqlDatabaseConnection connection = new(DatabaseSettings.ConnectionString);
            await connection.OpenAsync();

            using SqlDatabaseCommand command = connection.CreateCommand() as SqlDatabaseCommand;
            command.CommandText = $@"select {Name}, {TowersetUserId}, {Validity} from {Table};";

            using SqlDatabaseDataReader reader = await command.ExecuteReaderAsync() as SqlDatabaseDataReader;

            List<DescriptiveEntry> descriptiveEntries = new();
            while (await reader.ReadAsync())
            {
                string name   = await reader.GetFieldValueAsync<string>(Name);
                string userId = await reader.GetFieldValueAsync<string>(TowersetUserId);
                bool isValid  = await reader.GetFieldValueAsync<int>(Validity) != 0;
                
                DescriptiveEntry currentDescriptiveEntry = new(new(name, userId), isValid);
                
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
            command.CommandText = $@"delete from {Table}
                where {Name} = @{Name} and {TowersetUserId} = @{TowersetUserId};";
            command.Parameters.Add($"@{Name}", DbType.String).Value = key.name;
            command.Parameters.Add($"@{TowersetUserId}", DbType.String).Value = key.userId;

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

        public struct DataEntry
        {
            public bool isValid;
            public byte[] data;

            public DataEntry(bool isValid, byte[] data)
            {
                this.isValid = isValid;
                this.data = data;
            }
        }

        public struct DescriptiveEntry
        {
            public Identity identity;
            public bool isValid;

            public DescriptiveEntry(Identity identity, bool isValid)
            {
                this.identity = identity;
                this.isValid = isValid;
            }
        }
    }
}