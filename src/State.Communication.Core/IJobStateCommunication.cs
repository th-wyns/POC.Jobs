using System.Threading.Tasks;

namespace POC.Jobs.State.Communication
{
    /// <summary>
    /// Interface for job state communication.
    /// </summary>
    public interface IJobStateCommunication
    {
        /// <summary>
        /// Starts the communication.
        /// </summary>
        /// <returns></returns>
        Task StartAsync();

        /// <summary>
        /// Sends the create state.
        /// </summary>
        /// <param name="jobState">State of the job.</param>
        /// <returns></returns>
        Task SendCreateAsync(JobState jobState);

        /// <summary>
        /// Sends the update state.
        /// </summary>
        /// <param name="jobState">State of the job.</param>
        /// <returns></returns>
        Task SendUpdateAsync(JobState jobState);
    }
}
