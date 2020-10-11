using System.Threading.Tasks;

namespace POC.Jobs.State.Communication.SignalR
{
    /// <summary>
    /// Interface for SignalR clients.
    /// </summary>
    public interface IJobStateClient
    {
        /// <summary>
        /// Receives the job state create notification.
        /// </summary>
        /// <param name="jobState">State of the job.</param>
        /// <returns></returns>
        Task ReceiveJobStateCreateNotification(object jobState);

        /// <summary>
        /// Receives the job state update notification.
        /// </summary>
        /// <param name="jobState">State of the job.</param>
        /// <returns></returns>
        Task ReceiveJobStateUpdateNotification(object jobState);
    }
}
