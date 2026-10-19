using System;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.RentVehicle
{
    /// <summary>
    /// Input message of the rent vehicle use case.
    /// </summary>
    /// <param name="vehicleId">Identifier of the vehicle to rent.</param>
    /// <param name="customerId">Identifier of the person who rents.</param>
    public sealed class RentVehicleInput(Guid vehicleId, string customerId) : IUseCaseInput
    {
        /// <summary>
        /// Gets the identifier of the vehicle to rent.
        /// </summary>
        public Guid VehicleId { get; } = vehicleId;

        /// <summary>
        /// Gets the identifier of the person who rents.
        /// </summary>
        public string CustomerId { get; } = customerId;
    }
}
