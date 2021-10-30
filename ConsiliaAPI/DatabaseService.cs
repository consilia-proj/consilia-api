using Npgsql;

namespace ConsiliaAPI
{
    public static class DatabaseService
    {
        private static readonly string PostgresUsername = "postgres";
        private static readonly string PostgresPassword = "Password123!";
        private static readonly string PostgresDatabaseName = "consilia-dev";
        private static readonly string PostgresAddress = "34.132.192.247";
        
        private static NpgsqlConnectionStringBuilder PostgresConnectionString = new NpgsqlConnectionStringBuilder()
        {
            Database = PostgresDatabaseName,
            Username = PostgresUsername,
            Password = PostgresPassword,
            Host = PostgresAddress,
            MinPoolSize = 5,
            ApplicationName = "consilia-api",
            Pooling = true
        };
    }
}