using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace POC.Jobs.State.Communication.SignalR
{
    /// <summary>
    /// SignalR hub for state communication.
    /// </summary>
    /// <seealso cref="POC.Jobs.State.Communication.SignalR.IJobStateHub" />
    public class JobStateHub : Hub<IJobStateClient>, IJobStateHub
    {
        /// <summary>
        /// Sends the create notification.
        /// </summary>
        /// <param name="jobState">State of the job.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">jobState</exception>
        public Task SendCreateNotification(JobState jobState)
        {
            _ = jobState ?? throw new ArgumentNullException(nameof(jobState));

            Console.WriteLine($"Got create notification for job state id: {jobState?.Id}");
            // load job state and forward
            // TODO: filter for user / project group
            return Clients.All.ReceiveJobStateCreateNotification(jobState!);
        }

        /// <summary>
        /// Sends the update notification.
        /// </summary>
        /// <param name="jobState">State of the job.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">jobState</exception>
        public Task SendUpdateNotification(JobState jobState)
        {
            _ = jobState ?? throw new ArgumentNullException(nameof(jobState));

            Console.WriteLine($"Got update notification for job state id: {jobState?.Id}");
            // load job state and forward
            // TODO: filter for user / project group
            return Clients.All.ReceiveJobStateUpdateNotification(jobState!);
        }
    }
}
