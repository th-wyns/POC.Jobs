using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using POC.Jobs.Storage;

namespace POC.Jobs.Manager
{
    /// <summary>
    /// Job manager implementation.
    /// </summary>
    public class JobManager
    {
        IJobStore managementStore;

        JobStateManager stateManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="JobManager"/> class.
        /// </summary>
        /// <param name="managementStore">The management store.</param>
        /// <param name="stateManager">The state manager.</param>
        public JobManager(IJobStore managementStore, JobStateManager stateManager)
        {
            this.managementStore = managementStore;
            this.stateManager = stateManager;
        }

        /// <summary>
        /// Starts the processing.
        /// </summary>
        public void StartProcessing()
        {
            managementStore.StartProcessing();
        }

        /// <summary>
        /// Cancels the job.
        /// </summary>
        /// <param name="jobStateId">The job state identifier.</param>
        public async Task CancelAsync(int jobStateId)
        {
            var jobState = await stateManager.FindByIdAsync(jobStateId).ConfigureAwait(true);
            if (jobState.Status == Status.Running)
            {
                managementStore.Cancel(jobState.JobId!);
                await stateManager.UpdateAsync(new JobStateUpdate { Id = jobStateId, Status = Status.Cancelling }).ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Finds the job by identifier.
        /// </summary>
        /// <param name="jobStateId">The job state identifier.</param>
        /// <returns></returns>
        public async Task<JobState> FindByIdAsync(long jobStateId)
        {
            return await stateManager.FindByIdAsync(jobStateId).ConfigureAwait(true);
        }

        /// <summary>
        /// Pauses the job.
        /// </summary>
        /// <param name="jobStateId">The job state identifier.</param>
        public async Task PauseAsync(int jobStateId)
        {
            var jobState = await stateManager.FindByIdAsync(jobStateId).ConfigureAwait(true);
            if (jobState.Status == Status.Running || jobState.Status == Status.Resuming)
            {
                managementStore.Pause(jobState.JobId!);
                await stateManager.UpdateAsync(new JobStateUpdate { Id = jobStateId, Status = Status.Pausing }).ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Queues the job.
        /// </summary>
        /// <param name="job">The job.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">job</exception>
        public async Task<long> QueueAsync(Job job)
        {
            _ = job ?? throw new ArgumentNullException(nameof(job));
            var jobStateId = await stateManager.CreateAsync(job).ConfigureAwait(true);
            job.JobStateId = jobStateId;
            var jobId = managementStore.Queue(job);
            await stateManager.UpdateAsync(new JobStateUpdate { Id = jobStateId, JobId = jobId, Status = Status.Queued }).ConfigureAwait(true);
            return jobStateId;
        }

        /// <summary>
        /// Resumes the job.
        /// </summary>
        /// <param name="jobStateId">The job state identifier.</param>
        public async Task ResumeAsync(int jobStateId)
        {
            var jobState = await stateManager.FindByIdAsync(jobStateId).ConfigureAwait(true);
            if (jobState.Status == Status.Paused || jobState.Status == Status.Pausing)
            {
                managementStore.Resume(jobState.JobId!);
                await stateManager.UpdateAsync(new JobStateUpdate { Id = jobStateId, Status = Status.Resuming }).ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Finds all jobs.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public async Task<IList<JobState>> FindAllAsync(JobQueryOptions options)
        {
            return await Task.FromResult(stateManager.FindAll(options)).ConfigureAwait(true);
        }

        /// <summary>
        /// Finds the jobs by project identifier.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public async Task<IList<JobState>> FindByProjectIdAsync(string projectId, JobQueryOptions options)
        {
            return await Task.FromResult(stateManager.FindByProject(projectId, options)).ConfigureAwait(true);
        }
    }
}
