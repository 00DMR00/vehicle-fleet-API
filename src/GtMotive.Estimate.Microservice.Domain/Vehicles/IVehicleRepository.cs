using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GtMotive.Estimate.Microservice.Domain.Vehicles
{
    /// <summary>
    /// Persistence port for the <see cref="Vehicle"/> aggregate.
    /// </summary>
    public interface IVehicleRepository
    {
        /// <summary>
        /// Stores a new vehicle.
        /// </summary>
        /// <param name="vehicle">Vehicle to store.</param>
        /// <returns>A task that completes when the operation has finished.</returns>
        Task Add(Vehicle vehicle);

        /// <summary>
        /// Gets a vehicle by its identifier.
        /// </summary>
        /// <param name="id">Vehicle identifier.</param>
        /// <returns>The vehicle, or null when it does not exist.</returns>
        Task<Vehicle> GetById(Guid id);

        /// <summary>
        /// Gets the vehicles that are not currently rented.
        /// </summary>
        /// <returns>The available vehicles.</returns>
        Task<IReadOnlyCollection<Vehicle>> GetAvailable();

        /// <summary>
        /// Persists the changes of an existing vehicle.
        /// </summary>
        /// <param name="vehicle">Vehicle to update.</param>
        /// <returns>A task that completes when the operation has finished.</returns>
        Task Update(Vehicle vehicle);

        /// <summary>
        /// Checks whether a vehicle with the given license plate already exists.
        /// </summary>
        /// <param name="licensePlate">Normalized license plate.</param>
        /// <returns>True when the license plate is already registered.</returns>
        Task<bool> ExistsByLicensePlate(string licensePlate);
    }
}
