namespace POC.Jobs.Manager
{
    /// <summary>
    /// Interface for job store.
    /// </summary>
    public interface IJobStore
    {
        /// <summary>
        /// Starts the processing.
        /// </summary>
        void StartProcessing();

        /// <summary>
        /// Queues the specified job.
        /// </summary>
        /// <param name="job">The job.</param>
        /// <returns></returns>
        string Queue(Job job);

        /// <summary>
        /// Pauses the specified job identifier.
        /// </summary>
        /// <param name="jobId">The job identifier.</param>
        void Pause(string jobId);

        /// <summary>
        /// Resume job.
        /// </summary>
        /// <param name="jobId">The job identifier.</param>
#pragma warning disable CA1716 // Identifiers should not match keywords
        void Resume(string jobId);

        /// <summary>
        /// Cancel the job.
        /// </summary>
        /// <param name="jobId">The job identifier.</param>
#pragma warning restore CA1716 // Identifiers should not match keywords
        void Cancel(string jobId);

        /// <summary>
        /// Gets the type of the job.
        /// </summary>
        /// <param name="jobId">The job identifier.</param>
        /// <returns></returns>
        string GetJobType(string jobId);

        /// <summary>
        /// History of the specified job.
        /// </summary>
        /// <param name="jobId">The job identifier.</param>
        void History(string jobId);
    }
}
