using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using POC.Jobs.State.Communication;

namespace POC.Jobs.Storage
{
    /// <summary>
    /// Job state manager implementation.
    /// </summary>
    public class JobStateManager
    {
        IJobStateStore stateStore;
        IJobStateCommunication stateCommunication;

        /// <summary>
        /// Initializes a new instance of the <see cref="JobStateManager"/> class.
        /// </summary>
        /// <param name="stateStore">The state store.</param>
        /// <param name="stateCommunication">The state communication.</param>
        public JobStateManager(IJobStateStore stateStore, IJobStateCommunication stateCommunication)
        {
            this.stateStore = stateStore;
            this.stateCommunication = stateCommunication;
            // TODO: do not wait
            stateCommunication?.StartAsync().Wait();
        }

        /// <summary>
        /// Creates the state entry.
        /// </summary>
        /// <param name="job">The job.</param>
        /// <returns></returns>
        public async Task<long> CreateAsync(Job job)
        {
            var jobState = stateStore.Create(job);
            await stateCommunication.SendCreateAsync(jobState).ConfigureAwait(true);
            return jobState.Id;
        }

        /// <summary>
        /// Finds the state by identifier.
        /// </summary>
        /// <param name="jobStateId">The job state identifier.</param>
        /// <returns></returns>
        public async Task<JobState> FindByIdAsync(long jobStateId)
        {
            return await stateStore.FindByIdAsync(jobStateId).ConfigureAwait(true);
        }

        /// <summary>
        /// Finds the state by owner.
        /// </summary>
        /// <param name="ownerId">The owner identifier.</param>
        /// <returns></returns>
        public IEnumerable<JobState> FindByOwner(string ownerId)
        {
            return stateStore.FindByOwner(ownerId);
        }

        /// <summary>
        /// Finds the state by project.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public IList<JobState> FindByProject(string projectId, JobQueryOptions options)
        {
            return stateStore.FindByProject(projectId, options);
        }

        /// <summary>
        /// Finds all state that matches the given options.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public IList<JobState> FindAll(JobQueryOptions options)
        {
            return stateStore.FindAll(options);
        }

        /// <summary>
        /// Updates the state.
        /// </summary>
        /// <param name="stateUpdate">The state update.</param>
        /// <exception cref="ArgumentNullException">stateUpdate</exception>
        public async Task UpdateAsync(JobStateUpdate stateUpdate)
        {
            _ = stateUpdate ?? throw new ArgumentNullException(nameof(stateUpdate));
            await stateStore.UpdateAsync(stateUpdate).ConfigureAwait(true);
            var state = await stateStore.FindByIdAsync(stateUpdate.Id).ConfigureAwait(true);
            await stateCommunication.SendUpdateAsync(state).ConfigureAwait(true);
        }
    }
}
