using System;
using System.ComponentModel;
using Hangfire;

namespace POC.Jobs.Agent.HangfireSignalR
{
    /// <summary>
    /// Activator for Agent type.
    /// </summary>
    /// <seealso cref="Hangfire.JobActivator" />
    public class AgentJobActivator : JobActivator
    {
        private IContainer? _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentJobActivator"/> class.
        /// </summary>
        public AgentJobActivator()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentJobActivator"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public AgentJobActivator(IContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// Activates the agent.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public override object ActivateJob(Type type)
        {
            return IoC.Get<IAgent>();
        }
    }
}
