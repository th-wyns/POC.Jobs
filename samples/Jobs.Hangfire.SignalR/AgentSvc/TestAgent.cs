using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using POC.Jobs.Agent.HangfireSignalR;
using POC.Jobs.Storage;

namespace POC.Jobs.Samples.AgentSvc
{
    /// <summary>
    /// Test agent implementation.
    /// </summary>
    /// <seealso cref="POC.Jobs.Agent.HangfireSignalR.HangfireSignalrAgentBase" />
    public class TestAgent : HangfireSignalrAgentBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestAgent"/> class.
        /// </summary>
        /// <param name="stateManager">The state manager.</param>
        /// <param name="logger">The logger.</param>
        public TestAgent(JobStateManager stateManager, ILogger<TestAgent> logger) : base(stateManager, logger)
        {
        }

        /// <summary>
        /// Gets the agent type.
        /// </summary>
        /// <value>
        /// The agent type.
        /// </value>
        public override string Type => JobType.Test.GetDescription();

        /// <summary>
        /// Gets the agent version.
        /// </summary>
        /// <value>
        /// The agent version.
        /// </value>
        public override string Version => "v1.0";

        /// <summary>
        /// Executes the job. Entrypoint for agent on execution.
        /// </summary>
        /// <param name="job">The job.</param>
        /// <param name="jobState">State of the job.</param>
        /// <param name="isResuming">if set to <c>true</c> the job was prevoiusly paused and now it is resumed.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="ArgumentNullException">
        /// job
        /// or
        /// jobState
        /// </exception>
        public override async Task ExecuteJobAsync(Job job, JobState jobState, bool isResuming, CancellationToken cancellationToken)
        {
            _ = job ?? throw new ArgumentNullException(nameof(job));
            _ = jobState ?? throw new ArgumentNullException(nameof(jobState));

            Console.WriteLine($"Processing job with job state id {job.JobStateId}: {JsonConvert.SerializeObject(job)}");

            var startFrom = string.IsNullOrEmpty(jobState.Progress) ? 1 : int.Parse(jobState.Progress.Split('/')[0], System.Globalization.CultureInfo.InvariantCulture);

            for (int i = startFrom; i <= 10; i++)
            {
                Thread.Sleep(1000);
                var abortJob = await UpdateProgressAsync(job.JobStateId, 10, i, 0).ConfigureAwait(true);
                if (abortJob)
                {
                    await HandleAbortAsync(job.JobStateId).ConfigureAwait(true);
                    Console.WriteLine($"Aborted job: {job.JobStateId}");
                    return;
                }
            }
            await HandleSuccessAsync(job.JobStateId).ConfigureAwait(true);

            Console.WriteLine($"Processed job: {job.JobStateId}");
        }

        /// <summary>
        /// Starts this agent.
        /// </summary>
        public override void Start()
        {
            Console.WriteLine("Starting Agent...");
            base.Start();
            Console.WriteLine("Background Job Agent started. Press any key to exit...");
            Console.ReadKey();
        }

        private async Task<bool> UpdateProgressAsync(long jobStateId, int total, int results, int error)
        {
            Logger.LogDebug($"Updating job progress with job state id {jobStateId}");

            var update = new JobStateUpdate
            {
                Id = jobStateId,
                Results = results,
                Errors = error,
                Total = total,
                Progress = $"{results}/{total}",
            };
            return await UpdateStateAsync(update).ConfigureAwait(true);
        }
    }
}
