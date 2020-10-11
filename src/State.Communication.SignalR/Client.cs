using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace POC.Jobs.State.Communication.SignalR
{
    /// <summary>
    /// SignalR implementation of job state communication.
    /// </summary>
    /// <seealso cref="POC.Jobs.State.Communication.IJobStateCommunication" />
    public class SignalrClient : IJobStateCommunication
    {
        // https://docs.microsoft.com/en-us/aspnet/core/signalr/dotnet-client?view=aspnetcore-3.1&tabs=visual-studio

        HubConnection? connection;

        /// <summary>
        /// Starts the communication.
        /// </summary>
        public async Task StartAsync()
        {
            var hubUrl = Configuration.SignalrHubUrl;
            Console.WriteLine($"Connecting to {hubUrl}");
            connection = new HubConnectionBuilder()
                .WithUrl(hubUrl)
                .WithAutomaticReconnect(new[] { TimeSpan.Zero, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10) })
                .Build();

            await connection.StartAsync().ConfigureAwait(true);

            // test
            //Listen();
        }

        /// <summary>
        /// Sends the create state.
        /// </summary>
        /// <param name="jobState">State of the job.</param>
        public async Task SendCreateAsync(JobState jobState)
        {
            await connection.InvokeAsync("SendCreateNotification", jobState).ConfigureAwait(true);
        }

        /// <summary>
        /// Sends the update state.
        /// </summary>
        /// <param name="jobState">State of the job.</param>
        public async Task SendUpdateAsync(JobState jobState)
        {
            await connection.InvokeAsync("SendUpdateNotification", jobState).ConfigureAwait(true);
        }

        //void Listen()
        //{
        //    connection.On<long>("ReceiveJobStateUpdateNotification", (jobStateId) =>
        //    {
        //        Console.WriteLine($"SignalR: {jobStateId}");
        //    });
        //}
    }
}
