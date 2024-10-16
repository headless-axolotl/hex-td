using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using SQLDatabase.Net.SQLDatabaseClient;

namespace Lotl.Data
{
    using Parameter = SqlDatabaseParameter;

    public class UserContext
        : IContext<UserContext.DataEntry, UserContext.DescriptiveEntry, string>
    {
        #region Constants

        private string Table        => DatabaseSettings.UserTable;
        private string Id           => DatabaseSettings.UserId;
        private string PasswordHash => DatabaseSettings.UserPasswordHash;
        private string Data         => DatabaseSettings.UserData;

        #endregion

        private readonly DatabaseContext databaseContext;

        public UserContext(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public async Task CreateTableAsync()
        {
            try
            {
                string query = $@"create table if not exists {Table}(
                    {Id} text not null,
                    {PasswordHash} text not null,
                    {Data} blob not null,
                    primary key({Id}) on conflict replace
                );";

                databaseContext.CreateCommand(query);
                await databaseContext.ExecuteNonQueryAsync();
            }
            catch (Exception) { throw; }
            finally { await databaseContext.CloseConnectionAsync(); }
        }

        public async Task SetAsync(string key, DataEntry data)
        {
            try
            {
                string query = $@"insert into {Table} values(
                @{Id}, @{PasswordHash}, @{Data});";

                Parameter id           = new($"@{Id}",           DbType.String) { Value = key };
                Parameter passwordHash = new($"@{PasswordHash}", DbType.String) { Value = data.passwordHash };
                Parameter _data        = new($"@{Data}",         DbType.Binary) { Value = data.data };

                databaseContext.CreateCommand(query);
                await databaseContext.ExecuteNonQueryAsync(id, passwordHash, _data);
            }
            catch (Exception) { throw; }
            finally { await databaseContext.CloseConnectionAsync(); }
        }

        public async Task<DataEntry> ReadDataAsync(string key)
        {
            try
            {
                string query = $@"select {PasswordHash}, {Data}
                from {Table} where {Id} = @{Id};";

                Parameter id = new($"@{Id}", DbType.String) { Value = key };

                databaseContext.CreateCommand(query);
                using SqlDatabaseDataReader reader = await databaseContext.ExecuteReaderAsync(id);

                await reader.ReadAsync();

                string passwordHash = await reader.GetFieldValueAsync<string>(PasswordHash);
                byte[] data = await reader.GetFieldValueAsync<byte[]>(Data);

                DataEntry dataEntry = new(passwordHash, data);

                await reader.CloseAsync();
                return dataEntry;
            }
            catch (Exception) { throw; }
            finally { await databaseContext.CloseConnectionAsync(); }
        }

        public async Task<List<DescriptiveEntry>> ReadAllAsync()
        {
            try
            {
                string query = $@"select {Id}, {PasswordHash} from {Table};";

                databaseContext.CreateCommand(query);
                using SqlDatabaseDataReader reader = await databaseContext.ExecuteReaderAsync();

                List<DescriptiveEntry> descriptiveEntries = new();
                while (await reader.ReadAsync())
                {
                    string id = await reader.GetFieldValueAsync<string>(Id);
                    string passwordHash = await reader.GetFieldValueAsync<string>(PasswordHash);

                    DescriptiveEntry currentDescriptiveEntry = new(id, passwordHash);

                    descriptiveEntries.Add(currentDescriptiveEntry);
                }

                await reader.CloseAsync();
                return descriptiveEntries;
            }
            catch (Exception) { throw; }
            finally { await databaseContext.CloseConnectionAsync(); }
        }

        public async Task DeleteAsync(string key)
        {
            try
            {
                string query = $"delete from {Table} where {Id} = @{Id};";
                Parameter id = new($"@{Id}", DbType.String) { Value = key };

                databaseContext.CreateCommand(query);
                await databaseContext.ExecuteNonQueryAsync(id);
            }
            catch (Exception) { throw; }
            finally { await databaseContext.CloseConnectionAsync(); }
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