using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace POC.Jobs.Storage.EntityFrameworkCore
{
    /// <summary>
    /// Store for job state.
    /// </summary>
    /// <seealso cref="POC.Jobs.Storage.IJobStateStore" />
    public class JobStateStore : IJobStateStore
    {
        private bool _disposed = false;

        // TODO
        //JobStateDbContext context => new JobStateDbContext(new DbContextOptions<JobStateDbContext>());
        readonly JobStateDbContext context = new JobStateDbContext(new DbContextOptions<JobStateDbContext>());

        /// <summary>
        /// Creates the specified job.
        /// </summary>
        /// <param name="job">The job.</param>
        /// <returns></returns>
        public JobState Create(Job job)
        {
            var state = context.JobStates.Add(JobState.FromJob(job));
            context.SaveChanges();
            return state.Entity;
        }

        /// <summary>
        /// Finds the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<JobState> FindByIdAsync(long id)
        {
            var isCached = context.ChangeTracker.Entries<JobState>().Any(it => it.Entity.Id == id);
            var jobState = await context.JobStates.FindAsync(id);
            if (isCached)
            {
                await context.Entry(jobState).ReloadAsync().ConfigureAwait(true);
            }
            return jobState;
        }

        //public async Task<JobState> FindByJobIdAsync(string jobId)
        //{
        //    return await context.JobStates.FirstOrDefaultAsync(js => jobId == js.JobId);
        //}

        /// <summary>
        /// Finds the by owner.
        /// </summary>
        /// <param name="owenerId">The owener identifier.</param>
        /// <returns></returns>
        public IEnumerable<JobState> FindByOwner(string owenerId)
        {
            return context.JobStates.Where(js => js.OwnerId == owenerId);
        }

        /// <summary>
        /// Finds the by project.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public IList<JobState> FindByProject(string projectId, JobQueryOptions options)
        {
            return context.JobStates.Where(js => js.ProjectId == projectId).ToList();
        }

        /// <summary>
        /// Finds all.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public IList<JobState> FindAll(JobQueryOptions options)
        {
            return context.JobStates.Where(js => options.Statuses.ToArray().Contains(js.Status)).ToList();
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="stateUpdate">The state update.</param>
        public async Task UpdateAsync(JobStateUpdate stateUpdate)
        {
            _ = stateUpdate ?? throw new ArgumentNullException(nameof(stateUpdate));

            var jobState = await FindByIdAsync(stateUpdate.Id).ConfigureAwait(true);
            if (stateUpdate.JobId != null)
            {
                jobState.JobId = stateUpdate.JobId;
            }
            if (stateUpdate.Results != null)
            {
                jobState.Results = stateUpdate.Results.Value;
            }
            if (stateUpdate.Errors != null)
            {
                jobState.Errors = stateUpdate.Errors.Value;
            }
            if (stateUpdate.Details != null)
            {
                jobState.Details = stateUpdate.Details;
            }
            if (stateUpdate.Progress != null)
            {
                jobState.Progress = stateUpdate.Progress;
            }
            if (stateUpdate.Total != null)
            {
                jobState.Total = stateUpdate.Total.Value;
            }
            if (stateUpdate.Status != null && stateUpdate.Status != jobState.Status)
            {
                jobState.Status = stateUpdate.Status.Value;
                switch (stateUpdate.Status)
                {
                    case Status.Canceled:
                    case Status.Failed:
                    case Status.Succeeded:
                        jobState.CompletionTime = DateTime.UtcNow;
                        break;
                }
            }
            jobState.ModificationTime = DateTime.UtcNow;

            context.Entry(jobState).State = EntityState.Modified;
            context.SaveChanges();
        }

        #region Dispose

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the role manager and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _disposed = true;
                context.Dispose();
            }
        }

        #endregion
    }
}
