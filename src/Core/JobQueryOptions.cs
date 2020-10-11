using System.Collections.Generic;

namespace POC.Jobs
{
    /// <summary>
    /// Query options for jobs.
    /// </summary>
    /// <seealso cref="POC.CollectionQueryOptions" />
    public sealed class JobQueryOptions : CollectionQueryOptions
    {
        /// <summary>
        /// The statuses to look for.
        /// </summary>
        /// <value>
        /// The statuses to look for.
        /// </value>
        public IList<Status> Statuses { get; } = new List<Status>();
    }
}
