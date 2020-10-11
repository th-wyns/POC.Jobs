using Microsoft.Extensions.DependencyInjection;
using POC.Jobs.Agent;

namespace POC.Jobs.Samples.AgentSvc
{
    class Program
    {
        static void Main(string[] _)
        {
            RegisterAgent();

            var agent = IoC.Get<TestAgent>();
            agent.Start();
        }

        private static void RegisterAgent()
        {
            Agent.HangfireSignalR.IoC.RegisterServices((services) =>
            {
                services.AddTransient(typeof(IAgent), typeof(TestAgent));
            }, JobType.Test);
        }
    }
}
