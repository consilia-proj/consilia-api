using System;
using Npgsql;

namespace ConsiliaAPI
{
    public static class Database
    {
        // Variables were directly-defined in source previously in order to get us running faster
        // (this was made for a hackathon) vars have now been changed.
        private static readonly string PostgresUsername = Environment.GetEnvironmentVariable("DB_USERNAME");
        private static readonly string PostgresPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
        private static readonly string PostgresDatabaseName = Environment.GetEnvironmentVariable("DB_NAME");
        private static readonly string PostgresAddress = Environment.GetEnvironmentVariable("DB_ADDRESS");
        private static readonly int PostgresPort = int.Parse(Environment.GetEnvironmentVariable("DB_PORT"));
        
        private static NpgsqlConnectionStringBuilder PostgresConnectionString = new NpgsqlConnectionStringBuilder()
        {
            Database = PostgresDatabaseName,
            Username = PostgresUsername,
            Password = PostgresPassword,
            Host = PostgresAddress,
            Port = PostgresPort,
            SslMode = SslMode.Require,
            RootCertificate = "./root.crt",
            MinPoolSize = 5,
            ApplicationName = "consilia-api",
            Pooling = true
        };
        
        public static NpgsqlConnection DatabaseConnection = new NpgsqlConnection(PostgresConnectionString.ToString());

        static Database()
        {
            DatabaseConnection.Open();
        }
    }
}