using System;
using System.Text.Json;
using System.Threading;
using POC.Jobs.Manager;

namespace POC.Jobs.Samples.ServerApp
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("SERVER1 STARTED");
            var manager = IoC.Get<JobManager>();
            manager.StartProcessing();

            while (true)
            {
                var job = new Job { Type = JobType.Test, Parameters = JsonSerializer.Serialize(new { Now = DateTime.UtcNow }), OwnerId = "User1", ProjectId = "Project1" };
                Console.WriteLine($"Queueing task: {JsonSerializer.Serialize(job)}");
                var jobStateId = manager.QueueAsync(job).Result;
                Console.WriteLine($"Queued task with jobStateId: {jobStateId}");
                Thread.Sleep(5000);
            }
        }
    }
}
