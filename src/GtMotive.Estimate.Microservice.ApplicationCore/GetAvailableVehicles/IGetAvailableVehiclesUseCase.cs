using System.Collections.Generic;
using System.Threading.Tasks;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.GetAvailableVehicles
{
    /// <summary>
    /// Gets vehicles filtered by rental status.
    /// </summary>
    public interface IGetAvailableVehiclesUseCase
    {
        /// <summary>
        /// Executes the query and returns vehicles matching the rental status.
        /// </summary>
        /// <param name="isRented">True for rented vehicles, false for available vehicles, or null for all vehicles.</param>
        /// <returns>The matching vehicles.</returns>
        Task<IReadOnlyCollection<AvailableVehicleOutput>> Execute(bool? isRented = null);
    }
}
