using LabSid.Infra.Interfaces;
using Microsoft.Data.Sqlite;
using System.Data;

namespace LabSid.Infra
{
    public class SqliteContext : ISqliteContext
    {
        private readonly string connectionString;
        private readonly IDbConnection connection;

        public SqliteContext(string connectionString)
        {
            this.connectionString = connectionString;
            connection = new SqliteConnection(this.connectionString);
            connection.Open();
        }

        public void Dispose()
        {
            connection.Close();
            connection.Dispose();
        }

        public IDbConnection GetConnection()
            => connection;

    }
}
