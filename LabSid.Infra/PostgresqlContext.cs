using LabSid.Infra.Interfaces;
using Npgsql;
using System.Data;

namespace LabSid.Infra
{
    public class PostgresqlContext : IPostgresqlContext
    {
        private readonly string connectionString;
        private readonly IDbConnection connection;

        public PostgresqlContext(string connectionString)
        {
            this.connectionString = connectionString;
            connection = new NpgsqlConnection(this.connectionString);
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
