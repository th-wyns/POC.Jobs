using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POC.Jobs.Storage
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface IJobStateStore : IDisposable
    {
        /// <summary>
        /// Creates the specified job.
        /// </summary>
        /// <param name="job">The job.</param>
        /// <returns></returns>
        JobState Create(Job job);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns></returns>
        Task UpdateAsync(JobStateUpdate state);

        /// <summary>
        /// Finds the by identifier asynchronous.
        /// </summary>
        /// <param name="jobStateId">The job state identifier.</param>
        /// <returns></returns>
        Task<JobState> FindByIdAsync(long jobStateId);

        /// <summary>
        /// Finds the by owner.
        /// </summary>
        /// <param name="ownerId">The owner identifier.</param>
        /// <returns></returns>
        IEnumerable<JobState> FindByOwner(string ownerId);

        /// <summary>
        /// Finds the by project.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        IList<JobState> FindByProject(string projectId, JobQueryOptions options);

        /// <summary>
        /// Finds all.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        IList<JobState> FindAll(JobQueryOptions options);
    }
}
