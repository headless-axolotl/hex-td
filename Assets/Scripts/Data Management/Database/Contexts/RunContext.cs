using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using SQLDatabase.Net.SQLDatabaseClient;

namespace Lotl.Data
{
    using Parameter = SqlDatabaseParameter;

    public class RunContext :
        IContext<byte[],  RunContext.Identity, RunContext.Identity>
    {
        #region Constants

        private string Table     => DatabaseSettings.RunTable;
        private string Name      => DatabaseSettings.RunName;
        private string RunUserId => DatabaseSettings.RunUserId;
        private string Data      => DatabaseSettings.RunData;

        #endregion

        private readonly DatabaseContext databaseContext;

        public RunContext(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public async Task CreateTableAsync()
        {
            try
            {
                string query = $@"create table if not exists {Table}(
                    {Name} text not null,
                    {RunUserId} text not null,
                    {Data} blob not null,
                    primary key ({Name}, {RunUserId}) on conflict replace,
                    foreign key ({RunUserId}) references {DatabaseSettings.UserTable}({DatabaseSettings.UserId})
                    on delete cascade on update no action
                );";
                databaseContext.CreateCommand(query);
                await databaseContext.ExecuteNonQueryAsync();
            }
            catch (Exception) { throw; }
            finally { await databaseContext.CloseConnectionAsync(); }
        }

        public async Task SetAsync(Identity key, byte[] data)
        {
            try
            {
                string query = $@"insert into {Table} values(
                    @{Name}, @{RunUserId}, @{Data});";

                Parameter name      = new($"@{Name}",      DbType.String) { Value = key.name };
                Parameter runUserId = new($"@{RunUserId}", DbType.String) { Value = key.userId };
                Parameter _data     = new($"@{Data}",      DbType.Binary) { Value = data };

                databaseContext.CreateCommand(query);
                await databaseContext.ExecuteNonQueryAsync(name, runUserId, _data);
            }
            catch (Exception) { throw; }
            finally { await databaseContext.CloseConnectionAsync(); }
        }

        public async Task<byte[]> ReadDataAsync(Identity key)
        {
            try
            {
                string query = $@"select {Data} from {Table} where {Name} = @{Name} and {RunUserId} = @{RunUserId};";

                Parameter name      = new($"@{Name}",      DbType.String) { Value = key.name };
                Parameter runUserId = new($"@{RunUserId}", DbType.String) { Value = key.userId };

                databaseContext.CreateCommand(query);

                using SqlDatabaseDataReader reader = await databaseContext.ExecuteReaderAsync(name, runUserId);
                await reader.ReadAsync();

                byte[] data = await reader.GetFieldValueAsync<byte[]>(Data);
                await reader.CloseAsync();
                return data;
            }
            catch (Exception) { throw; }
            finally { await databaseContext.CloseConnectionAsync(); }
        }

        public async Task<List<Identity>> ReadAllAsync()
        {
            try
            {
                string query = $@"select {Name}, {RunUserId} from {Table};";

                databaseContext.CreateCommand(query);

                using SqlDatabaseDataReader reader = await databaseContext.ExecuteReaderAsync();

                List<Identity> descriptiveEntries = new();

                while (await reader.ReadAsync())
                {
                    string name = await reader.GetFieldValueAsync<string>(Name);
                    string userId = await reader.GetFieldValueAsync<string>(RunUserId);

                    Identity currentDescriptiveEntry = new(name, userId);

                    descriptiveEntries.Add(currentDescriptiveEntry);
                }

                await reader.CloseAsync();
                return descriptiveEntries;
            }
            catch (Exception) { throw; }
            finally { await databaseContext.CloseConnectionAsync(); }
        }

        public async Task DeleteAsync(Identity key)
        {
            try
            {
                string query = $@"delete from {Table} where {Name} = @{Name} and {RunUserId} = @{RunUserId};";

                Parameter name      = new($"@{Name}",      DbType.String) { Value = key.name };
                Parameter runUserId = new($"@{RunUserId}", DbType.String) { Value = key.userId };

                databaseContext.CreateCommand(query);
                await databaseContext.ExecuteNonQueryAsync(name, runUserId);
            }
            catch (Exception) { throw; }
            finally { await databaseContext.CloseConnectionAsync(); }
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
