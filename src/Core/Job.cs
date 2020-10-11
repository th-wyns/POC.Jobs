namespace POC.Jobs
{

    /// <summary>
    /// Describes a job abstraction.
    /// </summary>
    public class Job
    {
        /// <summary>
        /// Gets or sets the job state identifier.
        /// </summary>
        /// <value>
        /// The job state identifier.
        /// </value>
        public long JobStateId { get; set; }
        /// <summary>
        /// Gets or sets the job type.
        /// </summary>
        /// <value>
        /// The job type.
        /// </value>
        public JobType? Type { get; set; }
        /// <summary>
        /// Gets or sets the job input parameters.
        /// </summary>
        /// <value>
        /// The job input parameters.
        /// </value>
        public string? Parameters { get; set; }
        /// <summary>
        /// Gets or sets the project identifier.
        /// </summary>
        /// <value>
        /// The project identifier.
        /// </value>
        public string? ProjectId { get; set; }
        /// <summary>
        /// Gets or sets the owner identifier.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        public string? OwnerId { get; set; }
    }
}
