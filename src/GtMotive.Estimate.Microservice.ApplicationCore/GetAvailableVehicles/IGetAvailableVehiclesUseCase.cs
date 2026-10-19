using System.Collections.Generic;
using System.Threading.Tasks;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.GetAvailableVehicles
{
    /// <summary>
    /// Gets the vehicles that are available for rent.
    /// </summary>
    public interface IGetAvailableVehiclesUseCase
    {
        /// <summary>
        /// Executes the query and returns the available vehicles.
        /// </summary>
        /// <returns>The available vehicles.</returns>
        Task<IReadOnlyCollection<AvailableVehicleOutput>> Execute();
    }
}
