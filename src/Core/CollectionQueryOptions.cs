namespace POC
{
    /// <summary>
    /// Provides abstraction for representing query options for pageable collections.
    /// </summary>
    public abstract class CollectionQueryOptions
    {
        /// <summary>
        /// Gets or sets the field name used for sorting the items.
        /// </summary>
        /// <value>
        /// The field name.
        /// </value>
        public string OrderBy { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether items should be retrieved in descending order.
        /// </summary>
        /// <value>
        ///   <c>true</c> if items should be retrieved in descending order; otherwise, <c>false</c>.
        /// </value>
        public bool OrderByDesc { get; set; }

        /// <summary>
        /// Gets or sets the number of rows to skip, before starting to return rows from the query expression.
        /// </summary>
        /// <value>
        /// The number of rows to skip.
        /// </value>
        public int? Offset { get; set; }

        /// <summary>
        /// Gets or sets the number of rows to return, after processing the offset clause.
        /// </summary>
        /// <value>
        /// The number of rows to return.
        /// </value>
        public int? Limit { get; set; }
    }
}
