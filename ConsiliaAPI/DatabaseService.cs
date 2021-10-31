using Npgsql;

namespace ConsiliaAPI
{
    public static class Database
    {
        private static readonly string PostgresUsername = "nicholas";//"postgres";
        private static readonly string PostgresPassword = "TwelveCharPass12!";// "Password123!";
        private static readonly string PostgresDatabaseName = "meaty-cougar-4640.defaultdb";
        private static readonly string PostgresAddress = "free-tier.gcp-us-central1.cockroachlabs.cloud"; //"34.132.192.247";
        
        private static NpgsqlConnectionStringBuilder PostgresConnectionString = new NpgsqlConnectionStringBuilder()
        {
            Database = PostgresDatabaseName,
            Username = PostgresUsername,
            Password = PostgresPassword,
            Host = PostgresAddress,
            Port = 26257,
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