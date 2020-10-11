using System;
using System.Threading;
using Hangfire;
using Microsoft.Extensions.Logging;
using POC.Jobs.Manager;
using POC.Jobs.Storage;

namespace POC.Jobs.Agent.HangfireSignalR
{
    /// <summary>
    /// Abstract base for Hangfire and SignalR based agents.
    /// </summary>
    /// <seealso cref="POC.Jobs.Agent.AgentBase" />
    public abstract class HangfireSignalrAgentBase : AgentBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HangfireSignalrAgentBase"/> class.
        /// </summary>
        /// <param name="stateManager">The state manager.</param>
        /// <param name="logger"></param>
        public HangfireSignalrAgentBase(JobStateManager stateManager, ILogger logger)
            : base(stateManager, logger)
        {
        }

        /// <summary>
        /// Starts this agent.
        /// </summary>
        public override void Start()
        {
            Manager.Hangfire.Config.Queue = Type;

            try
            {
                GlobalConfiguration.Configuration.UseActivator(new AgentJobActivator());

                var manager = IoC.Get<JobManager>();
                manager.StartProcessing();
            }
            catch (Exception ex)
            {
#pragma warning disable CA1303 // Do not pass literals as localized parameters
                Logger.LogError(ex, $"Unexpected error in HangfireSignalrAgentBase initialization.");
#pragma warning restore CA1303 // Do not pass literals as localized parameters
                throw;
            }

            Logger.LogInformation($"Agent '{Type}' started on {Environment.MachineName}.");
            Thread.Sleep(Timeout.Infinite);
        }
    }
}
