using System;
using System.Data.SqlClient;
using System.Globalization;
using Microsoft.Extensions.Configuration;

namespace POC.Jobs.Manager.Hangfire
{
    /// <summary>
    /// The configuration.
    /// </summary>
    public static class Config
    {
        static IConfiguration Current { get; }
        static Config()
        {
            Current = GetConfiguration();

            EnsureDbCreated();
        }

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        public static string ConnectionString => Current.GetConnectionString("BackgroundJob.Manager.Hangfire");

        /// <summary>
        /// Gets or sets the job queues to process.
        /// </summary>
        /// <value>
        /// The job queues to process.
        /// </value>
        public static string Queue { get; set; } = default!;

        static IConfiguration GetConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            return configurationBuilder.Build();
        }

        static void EnsureDbCreated()
        {
            var connectionStringBuilder = new SqlConnectionStringBuilder(ConnectionString);
            var databaseName = connectionStringBuilder.InitialCatalog;
            connectionStringBuilder.InitialCatalog = "master";
            var masterConnectionString = connectionStringBuilder.ConnectionString;

            using (var connection = new SqlConnection(masterConnectionString))
            {
                connection.Open();

#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
                using (var command = new SqlCommand(string.Format(CultureInfo.InvariantCulture,
                    @"IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'{0}') 
                                    CREATE DATABASE [{0}];
                      ", databaseName), connection))
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
