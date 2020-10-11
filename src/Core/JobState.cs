using System;

namespace POC.Jobs
{
    /// <summary>
    /// 
    /// </summary>
    public class JobState
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the job identifier.
        /// </summary>
        /// <value>
        /// The job identifier.
        /// </value>
        public string? JobId { get; set; }

        /// <summary>
        /// Gets or sets the owner identifier.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        public string? OwnerId { get; set; }

        /// <summary>
        /// Gets or sets the project identifier.
        /// </summary>
        /// <value>
        /// The project identifier.
        /// </value>
        public string? ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string? Type { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public Status Status { get; set; }

        /// <summary>
        /// Gets or sets the results.
        /// </summary>
        /// <value>
        /// The results.
        /// </value>
        public int Results { get; set; }

        /// <summary>
        /// Gets or sets the errors.
        /// </summary>
        /// <value>
        /// The errors.
        /// </value>
        public int Errors { get; set; }

        /// <summary>
        /// Gets or sets the errors.
        /// </summary>
        /// <value>
        /// The errors.
        /// </value>
        public int Total { get; set; }

        /// <summary>
        /// Gets or sets the progress.
        /// </summary>
        /// <value>
        /// The progress.
        /// </value>
        public string? Progress { get; set; }

        /// <summary>
        /// Gets or sets the details.
        /// </summary>
        /// <value>
        /// The details.
        /// </value>
        public string? Details { get; set; }

        /// <summary>
        /// Gets or sets the creation time.
        /// </summary>
        /// <value>
        /// The creation time.
        /// </value>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// Gets or sets the modification time.
        /// </summary>
        /// <value>
        /// The modification time.
        /// </value>
        public DateTime? ModificationTime { get; set; }

        /// <summary>
        /// Gets or sets the completion time.
        /// </summary>
        /// <value>
        /// The completion time.
        /// </value>
        public DateTime? CompletionTime { get; set; }

        /// <summary>
        /// Froms the job.
        /// </summary>
        /// <param name="job">The job.</param>
        /// <returns></returns>
        public static JobState FromJob(Job job)
        {
            return new JobState
            {
                CreationTime = DateTime.UtcNow,
                OwnerId = job?.OwnerId,
                ProjectId = job?.ProjectId,
                Status = Status.Registered,
                Type = job?.Type?.GetDescription()
            };
        }
    }
}
