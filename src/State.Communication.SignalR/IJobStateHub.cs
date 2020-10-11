using System.Threading.Tasks;

namespace POC.Jobs.State.Communication.SignalR
{
    interface IJobStateHub
    {
        Task SendCreateNotification(JobState jobStateId);
        Task SendUpdateNotification(JobState jobStateId);
    }
}
