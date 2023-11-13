using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLDatabase.Net.SQLDatabaseClient;

namespace Lotl.Data
{
    using Parameter = SqlDatabaseParameter;

    public class DatabaseContext : IDisposable
    {
        private SqlDatabaseConnection connection;
        private SqlDatabaseCommand command;

        public DatabaseContext()
        {
            connection = new(DatabaseSettings.ConnectionString);
        }

        public void CreateCommand(string query)
        {
            command?.Dispose();
            command = new SqlDatabaseCommand(query, connection);
        }

        public async Task<object> ExecuteScalar(params Parameter[] parameters)
        {
            AddParameters(parameters);

            await connection.OpenAsync();
            object result = await command.ExecuteScalarAsync();
            return result;
        }

        public async Task<SqlDatabaseDataReader> ExecuteReader(params Parameter[] parameters)
        {
            AddParameters(parameters);

            await connection.OpenAsync();
            return await command.ExecuteReaderAsync() as SqlDatabaseDataReader;
        }

        public async Task<int> ExecuteNonQuery(params Parameter[] parameters)
        {
            AddParameters(parameters);

            await connection.OpenAsync();
            return await command.ExecuteNonQueryAsync();
        }

        private void AddParameters(Parameter[] parameters)
        {
            if (parameters == null) return;
            foreach (Parameter parameter in parameters)
            {
                command.Parameters.Add(parameter);
            }
        }

        public async Task CloseConnection()
        {
            command?.Dispose();
            await connection.CloseAsync();
        }

        public void Dispose()
        {
            connection.Dispose();
        }
    }
}
