using System.Threading;
using System.Threading.Tasks;

namespace POC.Jobs.Agent
{
    /// <summary>
    /// Interface for the processing agents with the job entry point.
    /// </summary>
    public interface IAgent
    {
        /// <summary>
        /// Gets the agent type.
        /// </summary>
        /// <value>
        /// The agent type.
        /// </value>
        string Type { get; }

        /// <summary>
        /// Gets the agent version.
        /// </summary>
        /// <value>
        /// The agent version.
        /// </value>
        string Version { get; }


        /// <summary>
        /// Starts this agent.
        /// </summary>
        void Start();

        /// <summary>
        /// Processes the specified job.
        /// </summary>
        /// <param name="job">The job.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task Process(Job job, CancellationToken cancellationToken);
    }
}
