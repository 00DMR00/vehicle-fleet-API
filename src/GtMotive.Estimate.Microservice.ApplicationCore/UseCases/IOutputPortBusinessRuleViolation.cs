namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases
{
    /// <summary>
    /// Interface to define the Business Rule Violation Output Port.
    /// </summary>
    public interface IOutputPortBusinessRuleViolation
    {
        /// <summary>
        /// Informs that a business rule prevented the operation.
        /// </summary>
        /// <param name="message">Text description.</param>
        void BusinessRuleViolationHandle(string message);
    }
}
