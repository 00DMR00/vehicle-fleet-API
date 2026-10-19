using System;
using GtMotive.Estimate.Microservice.Domain.Rentals;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.RentVehicle
{
    /// <summary>
    /// Output message of the rent vehicle use case.
    /// </summary>
    public sealed class RentVehicleOutput : IUseCaseOutput
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RentVehicleOutput"/> class.
        /// </summary>
        /// <param name="rental">Rental that has been created.</param>
        public RentVehicleOutput(Rental rental)
        {
            ArgumentNullException.ThrowIfNull(rental);

            RentalId = rental.Id;
            VehicleId = rental.VehicleId;
            CustomerId = rental.CustomerId;
            StartDate = rental.StartDate;
        }

        /// <summary>
        /// Gets the rental identifier.
        /// </summary>
        public Guid RentalId { get; }

        /// <summary>
        /// Gets the identifier of the rented vehicle.
        /// </summary>
        public Guid VehicleId { get; }

        /// <summary>
        /// Gets the identifier of the person who rents.
        /// </summary>
        public string CustomerId { get; }

        /// <summary>
        /// Gets the moment the rental started.
        /// </summary>
        public DateTimeOffset StartDate { get; }
    }
}
