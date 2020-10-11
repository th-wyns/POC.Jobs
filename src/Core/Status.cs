namespace POC.Jobs
{
    /// <summary>
    /// Status of the job.
    /// </summary>
    public enum Status
    {
        /// <summary>
        /// The registered
        /// </summary>
        Registered = 1,
        /// <summary>
        /// The queued
        /// </summary>
        Queued = 2,
        /// <summary>
        /// The running
        /// </summary>
        Running = 3,
        /// <summary>
        /// The pausing
        /// </summary>
        Pausing = 4,
        /// <summary>
        /// The paused
        /// </summary>
        Paused = 5,
        /// <summary>
        /// The resuming
        /// </summary>
        Resuming = 6,
        /// <summary>
        /// The cancelling
        /// </summary>
        Cancelling = 7,
        /// <summary>
        /// The canceled
        /// </summary>
        Canceled = 8,
        /// <summary>
        /// The failed
        /// </summary>
        Failed = 9,
        /// <summary>
        /// The succeeded
        /// </summary>
        Succeeded = 10
    }
}
