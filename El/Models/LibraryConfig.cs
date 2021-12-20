namespace El.Models
{
    /// <summary>
    /// Library configuration options
    /// </summary>
    public class LibraryConfig
    {
        /// <summary>
        /// Root user path
        /// </summary>
        public string BasePath { get; set; } = string.Empty;

        /// <summary>
        /// Root data path
        /// </summary>
        public string DataBasePath { get; set; } = string.Empty;

        /// <summary>
        /// Library config
        /// </summary>
        public AppSettings? App { get; set; }
    }

    public record AppSettings
    {
        /// <summary>
        /// Min threads for pool. Default 4
        /// </summary>
        public int MinThreads { get; init; } = 4;

        /// <summary>
        /// Min IO threads for pool. Default 4
        /// </summary>
        public int MinIOThreads { get; init; } = 4;

        /// <summary>
        /// Max queue rows
        /// </summary>
        public int MaxQueueRows { get; set; } = 500;

        /// <summary>
        /// Timeout in seconds
        /// </summary>
        public int DbCmdTimeout { get; init; } = 5;

        /// <summary>
        /// Timeout in seconds
        /// </summary>
        public int ApiCmdTimeout { get; init; } = 15;

        /// <summary>
        /// max number of statements
        /// </summary>
        public int EFBatchSize { get; init; } = 50;


        /// <summary>
        /// Name of main DB
        /// </summary>
        public string DbNameMain { get; init; } = "blogging.db";
    }
}
