using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using POC.Jobs.Manager;
using POC.Jobs.Manager.Hangfire;
using POC.Jobs.State.Communication;
using POC.Jobs.State.Communication.SignalR;
using POC.Jobs.Storage;
using POC.Jobs.Storage.EntityFrameworkCore;

namespace POC.Jobs.Agent.HangfireSignalR
{
    /// <summary>
    /// IoC Container
    /// </summary>
    public static class IoC
    {
        /// <summary>
        /// Gets the services.
        /// </summary>
        /// <value>
        /// The services.
        /// </value>
        public static IServiceCollection Services { get; } = new ServiceCollection();

        /// <summary>
        /// Gets the service provider.
        /// </summary>
        /// <value>
        /// The service provider.
        /// </value>
        public static ServiceProvider ServiceProvider { get; private set; } = default!;

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public static IConfiguration Configuration { get; private set; } = default!;

        static IoC()
        {

        }

        /// <summary>
        /// Registers the services.
        /// </summary>
        /// <param name="serviceCollectionAction">The service collection action.</param>
        /// <param name="jobType">Type of the job.</param>
        public static void RegisterServices(Action<IServiceCollection> serviceCollectionAction, JobType jobType)
        {
            _ = serviceCollectionAction ?? throw new ArgumentNullException(nameof(serviceCollectionAction));

            Configuration = GetConfiguration();

            serviceCollectionAction(Services);

            Services
                .AddTransient<IJobStore, JobStore>()
                .AddTransient<IJobStateStore, JobStateStore>()
                .AddTransient<JobStateManager>()
                .AddTransient<JobManager>()
                .AddTransient<IJobStateCommunication, SignalrClient>()
                .AddSingleton<IConfiguration>(provider => Configuration);


            Services.AddLogging().AddOptions();
            Services.AddLogging(configure => { configure.AddSerilog(); });

            ServiceProvider = Services.BuildServiceProvider();
            Services.BuildServiceProvider();
        }

        private static IConfiguration GetConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            return configurationBuilder.Build();
        }

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Get<T>()
        {
            return ServiceProvider.GetService<T>();
        }
    }
}
