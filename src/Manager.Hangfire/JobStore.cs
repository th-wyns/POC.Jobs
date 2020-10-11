using System;
using System.Threading;
using Hangfire;
using POC.Jobs.Agent;
using HangfireJob = Hangfire.Common.Job;
using EnqueuedState = Hangfire.States.EnqueuedState;
using System.Linq;

namespace POC.Jobs.Manager.Hangfire
{
    /// <summary>
    /// Interface for job store.
    /// </summary>
    /// <seealso cref="POC.Jobs.Manager.IJobStore" />
    public class JobStore : IJobStore
    {

        // stores the running Hangfire server
        static BackgroundJobServer? BackgroundJobServer { get; } = BackgroundJobServerFactory.Get();

        static JobStore()
        {
            BackgroundJobServer = BackgroundJobServerFactory.Get();
        }

        /// <summary>
        /// Starts the processing.
        /// </summary>
        public void StartProcessing()
        {

        }

        /// <summary>
        /// Cancels the job.
        /// </summary>
        /// <param name="jobId">The job identifier.</param>
        public void Cancel(string jobId)
        {
            BackgroundJob.Delete(jobId);
        }

        /// <summary>
        /// Gets the type of the job.
        /// </summary>
        /// <param name="jobId">The job identifier.</param>
        /// <returns></returns>
        public string GetJobType(string jobId)
        {
            return JobStorage.Current.GetMonitoringApi().JobDetails(jobId).History.Last(h => h.StateName == "Enqueued").Data["Queue"];
        }

        /// <summary>
        /// History of the specified job.
        /// </summary>
        /// <param name="jobId">The job identifier.</param>
        /// <exception cref="NotImplementedException"></exception>
        public void History(string jobId)
        {
            throw new NotImplementedException();
            //return JobStorage.Current.GetMonitoringApi().JobDetails(jobId).History;
        }

        /// <summary>
        /// Pauses the specified job.
        /// </summary>
        /// <param name="jobId">The job identifier.</param>
        public void Pause(string jobId)
        {
            BackgroundJob.Delete(jobId);
        }

        /// <summary>
        /// Queues the specified job.
        /// </summary>
        /// <param name="job">The job.</param>
        /// <returns></returns>
        public string Queue(Job job)
        {
            // https://docs.hangfire.io/en/latest/background-methods/using-cancellation-tokens.html

            // TODO: cache
            var type = typeof(IAgent);
            var method = type.GetMethod("Process");
            return new BackgroundJobClient().Create(new HangfireJob(type, method, job, CancellationToken.None), new EnqueuedState(job?.Type?.GetDescription()));
        }

        /// <summary>
        /// Resumes the job.
        /// </summary>
        /// <param name="jobId">The job identifier.</param>
        public void Resume(string jobId)
        {
            var jobType = GetJobType(jobId);
            new BackgroundJobClient().ChangeState(jobId, new EnqueuedState(jobType));
        }
    }
}
