namespace El.Utils
{
    /// <summary>
    /// A class that is going to be a root class for all other classes in the project.
    /// This class and its successors realize IDisposable interface for deterministic 
    /// memory and resource management
    /// </summary>
    public abstract class ClassRoot : IDisposable, IAsyncDisposable
    {
        /// <summary>
        /// A method to realize IDisposable interface
        /// </summary>
        public void Dispose()
        {
            //Call internal dispose method
            Dispose(true);

            //Disconnect from garbage collection finalize queue
            System.GC.SuppressFinalize(this);
        }

        /// <summary>
        /// A Destructor of the class
        /// </summary>
        ~ClassRoot()
        {
            //Call internal dispose method
            Dispose(false);
        }

        public async ValueTask DisposeAsync()
        {
            //Call internal dispose method
            await DisposeAsync(true);

            //Disconnect from garbage collection finalize queue
            System.GC.SuppressFinalize(this);
        }

        /// <summary>
        /// A method to be realized in successors for implementing 
        /// resource and memory disposal.
        /// </summary>
        /// <param name="flag">flag marking if the Dispose is triggered by user or by system </param>
        protected abstract void Dispose(bool flag);

        /// <summary>
        /// A method to be realized in successors for implementing 
        /// resource and memory disposal.
        /// </summary>
        /// <param name="flag">flag marking if the Dispose is triggered by user or by system </param>
        protected abstract ValueTask DisposeAsync(bool flag);


    }
}
