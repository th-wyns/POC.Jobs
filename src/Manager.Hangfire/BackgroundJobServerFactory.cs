using System;
using Hangfire;
using Hangfire.SqlServer;

namespace POC.Jobs.Manager.Hangfire
{
    /// <summary>
    /// Created the Hangfire BackgroundJobServer instance.
    /// </summary>
    public static class BackgroundJobServerFactory
    {
        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns></returns>
        public static BackgroundJobServer? Get()
        {
            // set storage to be able to enqueue and manage jobs
            SetupJobStorage();

            // if there is any queue defined, start the server to pull jobs from the queue
            return SetupBackgroundJobServer();
        }

        private static void SetupJobStorage()
        {
            var connectionString = Config.ConnectionString;
            // TODO: remove after testing
            Console.WriteLine($"Hangfire DB ConnectionString: {connectionString}");
            JobStorage.Current = new SqlServerStorage(
                connectionString,
                new SqlServerStorageOptions
                {
                    // TODO: fine tune options
                    QueuePollInterval = TimeSpan.FromSeconds(3)
                });
        }

        private static BackgroundJobServer? SetupBackgroundJobServer()
        {
            var queue = Config.Queue;
            if (string.IsNullOrEmpty(queue))
            {
                return null;
            }

            var options = new BackgroundJobServerOptions()
            {
                // TODO: fine tune options
                Queues = new[] { queue },
                // https://docs.hangfire.io/en/latest/background-processing/configuring-degree-of-parallelism.html
                WorkerCount = Environment.ProcessorCount * 5
            };
            return new BackgroundJobServer(options);
        }
    }
}
