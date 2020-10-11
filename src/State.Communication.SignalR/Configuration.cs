using System;
using Microsoft.Extensions.Configuration;

namespace POC.Jobs.State.Communication.SignalR
{
    class Configuration
    {
        static IConfigurationRoot Current { get; }
        static Configuration()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            Current = configurationBuilder.Build();
        }

#pragma warning disable CA1303 // Do not pass literals as localized parameters
        public static string SignalrHubUrl => Current.GetSection("BackgroundJob:State.Communication.SignalR.Url").Get<string>() ?? throw new ArgumentException("Missing configuration: SignalR Hub Url is requried.");
#pragma warning restore CA1303 // Do not pass literals as localized parameters
    }
}
