using System;
using Microsoft.Extensions.Configuration;

namespace POC.Jobs.Storage.EntityFrameworkCore
{
    static class Configuration
    {
        static IConfigurationRoot Current { get; }

        static Configuration()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            Current = configurationBuilder.Build();
        }

        public static string ConnectionString => Current.GetConnectionString("BackgroundJob.State.EFC");
    }
}
