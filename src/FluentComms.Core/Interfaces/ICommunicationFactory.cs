namespace FluentComms.Core.Interfaces
{
    /// <summary>
    /// Defines the contract for a factory that creates ICommunication instances.
    /// </summary>
    public interface ICommunicationFactory
    {
        /// <summary>
        /// Creates a new communication message builder instance.
        /// </summary>
        /// <returns>A new instance of a class implementing ICommunication.</returns>
        ICommunication Create();
    }
}
