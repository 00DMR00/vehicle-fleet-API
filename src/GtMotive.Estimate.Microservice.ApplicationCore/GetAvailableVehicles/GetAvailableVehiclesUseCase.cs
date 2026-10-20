using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Vehicles;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.GetAvailableVehicles
{
    /// <summary>
    /// Retrieves vehicles matching the requested rental status.
    /// </summary>
    /// <param name="vehicleRepository">Vehicle repository.</param>
    public sealed class GetAvailableVehiclesUseCase(IVehicleRepository vehicleRepository) : IGetAvailableVehiclesUseCase
    {
        private readonly IVehicleRepository _vehicleRepository = vehicleRepository;

        /// <inheritdoc />
        public async Task<IReadOnlyCollection<AvailableVehicleOutput>> Execute(bool? isRented = null)
        {
            var vehicles = await _vehicleRepository.GetByRentalStatus(isRented);
            return Array.AsReadOnly(vehicles.Select(vehicle => new AvailableVehicleOutput(vehicle)).ToArray());
        }
    }
}
