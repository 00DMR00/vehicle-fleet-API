using System;
using System.ComponentModel.DataAnnotations;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.RentVehicle;

namespace GtMotive.Estimate.Microservice.Api.UseCases.RentVehicle
{
    /// <summary>
    /// Response returned when a vehicle has been rented.
    /// </summary>
    public sealed class RentVehicleResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RentVehicleResponse"/> class.
        /// </summary>
        /// <param name="output">Output of the use case.</param>
        public RentVehicleResponse(RentVehicleOutput output)
        {
            ArgumentNullException.ThrowIfNull(output);

            RentalId = output.RentalId;
            VehicleId = output.VehicleId;
            CustomerId = output.CustomerId;
            StartDate = output.StartDate;
        }

        /// <summary>
        /// Gets the rental identifier.
        /// </summary>
        [Required]
        public Guid RentalId { get; }

        /// <summary>
        /// Gets the identifier of the rented vehicle.
        /// </summary>
        [Required]
        public Guid VehicleId { get; }

        /// <summary>
        /// Gets the identifier of the person who rents.
        /// </summary>
        [Required]
        public string CustomerId { get; }

        /// <summary>
        /// Gets the moment the rental started.
        /// </summary>
        [Required]
        public DateTimeOffset StartDate { get; }
    }
}
