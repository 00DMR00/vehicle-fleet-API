using System.Threading.Tasks;

namespace GtMotive.Estimate.Microservice.Domain.Rentals
{
    /// <summary>
    /// Persistence port for the <see cref="Rental"/> entity.
    /// </summary>
    public interface IRentalRepository
    {
        /// <summary>
        /// Stores a new rental.
        /// </summary>
        /// <param name="rental">Rental to store.</param>
        /// <returns>A task that completes when the operation has finished.</returns>
        Task Add(Rental rental);

        /// <summary>
        /// Checks whether a customer has a rental that has not been closed yet.
        /// </summary>
        /// <param name="customerId">Customer identifier.</param>
        /// <returns>True when the customer has an active rental.</returns>
        Task<bool> ExistsActiveForCustomer(string customerId);
    }
}
