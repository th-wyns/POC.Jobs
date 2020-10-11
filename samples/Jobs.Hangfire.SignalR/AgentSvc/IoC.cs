using Microsoft.Extensions.DependencyInjection;
using POC.Jobs.Agent;
using POC.Jobs.Manager;
using POC.Jobs.Manager.Hangfire;
using POC.Jobs.State.Communication;
using POC.Jobs.State.Communication.SignalR;
using POC.Jobs.Storage;
using POC.Jobs.Storage.EntityFrameworkCore;

namespace POC.Jobs.Samples.AgentSvc
{
    static class IoC
    {
        public static IServiceCollection Services { get; } = new ServiceCollection();
        public static ServiceProvider ServiceProvider { get; }

        static IoC()
        {
            Services
                .AddTransient(typeof(IAgent), typeof(TestAgent))
                .AddTransient(typeof(TestAgent), typeof(TestAgent))
                .AddTransient(typeof(IJobStore), typeof(JobStore))
                .AddTransient(typeof(IJobStateStore), typeof(JobStateStore))
                .AddTransient(typeof(JobStateManager), typeof(JobStateManager))
                .AddTransient(typeof(JobManager), typeof(JobManager))
                .AddTransient(typeof(IJobStateCommunication), typeof(SignalrClient));
            Services.AddLogging();
            ServiceProvider = Services.BuildServiceProvider();
        }

        public static T Get<T>()
        {
            return ServiceProvider.GetService<T>();
        }
    }
}
