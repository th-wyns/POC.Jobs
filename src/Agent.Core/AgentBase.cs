using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using POC.Jobs.Storage;

namespace POC.Jobs.Agent
{
    /// <summary>
    /// The base for agents.
    /// </summary>
    /// <seealso cref="POC.Jobs.Agent.IAgent" />
    public abstract class AgentBase : IAgent
    {
        JobStateManager stateManager;

        /// <summary>
        /// Gets the agent type.
        /// </summary>
        /// <value>
        /// The agent type.
        /// </value>
        public abstract string Type { get; }
        /// <summary>
        /// Gets the agent version.
        /// </summary>
        /// <value>
        /// The agent version.
        /// </value>
        public abstract string Version { get; }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        public ILogger Logger { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentBase"/> class.
        /// </summary>
        /// <param name="stateManager">The state manager.</param>
        /// <param name="logger"></param>
        public AgentBase(JobStateManager stateManager, ILogger logger)
        {
            this.stateManager = stateManager;
            Logger = logger;
        }


        /// <summary>
        /// Starts this agent.
        /// </summary>
        public abstract void Start();

        /// <summary>
        /// Processes the specified job.
        /// </summary>
        /// <param name="job">The job.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async virtual Task Process(Job job, CancellationToken cancellationToken)
        {
            try
            {
                _ = job ?? throw new ArgumentNullException(nameof(job));
                var originalJobState = await GetStateAsync(job.JobStateId).ConfigureAwait(true);
                await HandleStartAsync(job.JobStateId).ConfigureAwait(true);
                var jobState = await GetStateAsync(job.JobStateId).ConfigureAwait(true);
                await ExecuteJobAsync(job, jobState, originalJobState.Status == Status.Resuming, cancellationToken).ConfigureAwait(true);
                await HandleSuccessAsync(job.JobStateId).ConfigureAwait(true);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                Logger.LogError(ex, $"Error while processing Job with Type: {job.Type}, Owner: {job.OwnerId}, Parameters: {JsonSerializer.Serialize(job.Parameters)}");
                await HandleFailureAsync(job.JobStateId).ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Executes the job.
        /// </summary>
        /// <param name="job">The job.</param>
        /// <param name="jobState">State of the job.</param>
        /// <param name="isResuming">if set to <c>true</c> [is resumed].</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public abstract Task ExecuteJobAsync(Job job, JobState jobState, bool isResuming, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the state.
        /// </summary>
        /// <param name="jobStateId">The job state identifier.</param>
        /// <returns></returns>
        protected async Task<JobState> GetStateAsync(long jobStateId)
        {
            return await stateManager.FindByIdAsync(jobStateId).ConfigureAwait(true);
        }

        /// <summary>
        /// Updates the state.
        /// </summary>
        /// <param name="update">The update.</param>
        /// <returns>
        /// True if the job should be aborted; otherwise false.
        /// </returns>
        protected async Task<bool> UpdateStateAsync(JobStateUpdate update)
        {
            _ = update ?? throw new ArgumentNullException(nameof(update));
            var jobState = await GetStateAsync(update.Id).ConfigureAwait(true);
            await stateManager.UpdateAsync(update).ConfigureAwait(true);
            var abortJob = jobState.Status != Status.Running && jobState.Status != Status.Resuming && jobState.Status != Status.Queued;
            return abortJob;
        }

        /// <summary>
        /// Handles the abortion.
        /// </summary>
        /// <param name="jobStateId">The job state identifier.</param>
        protected async Task HandleAbortAsync(long jobStateId)
        {
            var jobState = await stateManager.FindByIdAsync(jobStateId).ConfigureAwait(true);
            if (jobState.Status == Status.Pausing)
            {
                await stateManager.UpdateAsync(new JobStateUpdate { Id = jobStateId, Status = Status.Paused }).ConfigureAwait(true);
            }
            else if (jobState.Status == Status.Cancelling)
            {
                await stateManager.UpdateAsync(new JobStateUpdate { Id = jobStateId, Status = Status.Canceled }).ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Handles the successful completion.
        /// </summary>
        /// <param name="jobStateId">The job state identifier.</param>
        protected async Task HandleSuccessAsync(long jobStateId)
        {
            var jobState = await stateManager.FindByIdAsync(jobStateId).ConfigureAwait(true);
            if (jobState.Status == Status.Running)
            {
                await stateManager.UpdateAsync(new JobStateUpdate { Id = jobStateId, Status = Status.Succeeded }).ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Handles the failure scenario.
        /// </summary>
        /// <param name="jobStateId">The job state identifier.</param>
        protected async Task HandleFailureAsync(long jobStateId)
        {
            await stateManager.UpdateAsync(new JobStateUpdate { Id = jobStateId, Status = Status.Failed }).ConfigureAwait(true);
        }

        /// <summary>
        /// Handles the start event.
        /// </summary>
        /// <param name="jobStateId">The job state identifier.</param>
        protected async Task HandleStartAsync(long jobStateId)
        {
            await stateManager.UpdateAsync(new JobStateUpdate { Id = jobStateId, Status = Status.Running }).ConfigureAwait(true);
        }
    }
}
