using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLDatabase.Net.SQLDatabaseClient;
using System;

namespace Lotl.Data
{
    using Parameter = SqlDatabaseParameter;
    
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

        private readonly DatabaseContext databaseContext;

        public TowersetContext(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public async Task CreateTable()
        {
            try
            {
                string query = $@"create table if not exists {Table}(
                    {Name} text not null,
                    {TowersetUserId} text not null,
                    {Validity} integer not null,
                    {Data} blob not null,
                    primary key({Name}, {TowersetUserId}) on conflict replace,
                    foreign key({TowersetUserId}) references {DatabaseSettings.UserTable}({DatabaseSettings.UserId})
                    on delete cascade on update no action
                );";
                databaseContext.CreateCommand(query);
                await databaseContext.ExecuteNonQuery();
            }
            catch (Exception) { throw; }
            finally { await databaseContext.CloseConnection(); }
        }

        public async Task Set(Identity key, DataEntry data)
        {
            try
            {
                string query = $@"insert into {Table} values(
                    @{Name}, @{TowersetUserId}, @{Validity}, @{Data});";

                Parameter name           = new($"@{Name}",           DbType.String) { Value = key.name };
                Parameter towersetUserId = new($"@{TowersetUserId}", DbType.String) { Value = key.userId };
                Parameter validity       = new($"@{Validity}",       DbType.Int32 ) { Value = data.isValid ? 1 : 0};
                Parameter _data          = new($"@{Data}",           DbType.Binary) { Value = data.data };

                databaseContext.CreateCommand(query);
                await databaseContext.ExecuteNonQuery(name, towersetUserId, validity);
            }
            catch (Exception) { throw; }
            finally { await databaseContext.CloseConnection(); }
        }

        public async Task<DataEntry> ReadData(Identity key)
        {
            try
            {
                string query = $@"select {Name}, {TowersetUserId}, {Validity}, {Data}
                    from {Table} where {Name} = @{Name} and {TowersetUserId} = @{TowersetUserId};";

                Parameter name           = new($"@{Name}",           DbType.String) { Value = key.name };
                Parameter towersetUserId = new($"@{TowersetUserId}", DbType.String) { Value = key.userId };

                databaseContext.CreateCommand(query);

                using SqlDatabaseDataReader reader = await databaseContext.ExecuteReader(name, towersetUserId);
                await reader.ReadAsync();

                bool isValid = await reader.GetFieldValueAsync<int>(Validity) != 0;
                byte[] data  = await reader.GetFieldValueAsync<byte[]>(Data);

                DataEntry dataEntry = new(isValid, data);

                await reader.CloseAsync();
                return dataEntry;
            }
            catch (Exception) { throw; }
            finally { await databaseContext.CloseConnection(); }
        }

        public async Task<List<DescriptiveEntry>> ReadAll()
        {

            try
            {
                string query = $@"select {Name}, {TowersetUserId}, {Validity} from {Table};";

                databaseContext.CreateCommand(query);

                using SqlDatabaseDataReader reader = await databaseContext.ExecuteReader();

                List<DescriptiveEntry> descriptiveEntries = new();
                while (await reader.ReadAsync())
                {
                    string name = await reader.GetFieldValueAsync<string>(Name);
                    string userId = await reader.GetFieldValueAsync<string>(TowersetUserId);
                    bool isValid = await reader.GetFieldValueAsync<int>(Validity) != 0;

                    DescriptiveEntry currentDescriptiveEntry = new(new(name, userId), isValid);

                    descriptiveEntries.Add(currentDescriptiveEntry);
                }

                await reader.CloseAsync();

                return descriptiveEntries;
            }
            catch (Exception) { throw; }
            finally { await databaseContext.CloseConnection(); }
            
        }

        public async Task Delete(Identity key)
        {
            try
            {
                string query = $@"delete from {Table}
                where {Name} = @{Name} and {TowersetUserId} = @{TowersetUserId};";

                Parameter name = new($"@{Name}", DbType.String) { Value = key.name };
                Parameter towersetUserId = new($"@{TowersetUserId}", DbType.String) { Value = key.userId };

                databaseContext.CreateCommand(query);

                await databaseContext.ExecuteNonQuery(name, towersetUserId);
            }
            catch (Exception) { throw; }
            finally { await databaseContext.CloseConnection(); }
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