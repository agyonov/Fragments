using Db;

namespace El.Interfaces
{
    /// <summary>
    /// Interface to create database connections in a factory way
    /// </summary>
    public interface ILibObjectsFactory
    {
        /// <summary>
        /// The metod that creates the factory
        /// </summary>
        /// <returns>The newly created db context</returns>
        BloggingContext CreateInstanceDb();

        ///// <summary>
        ///// The instantantiation method
        ///// </summary>
        ///// <returns></returns>
        //SecurityTool CreateInstanceSecurityTool();
    }
}
