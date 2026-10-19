using System;
using System.ComponentModel.DataAnnotations;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.CreateVehicle;

namespace GtMotive.Estimate.Microservice.Api.UseCases.CreateVehicle
{
    /// <summary>
    /// Response returned when a vehicle has been added to the fleet.
    /// </summary>
    public sealed class CreateVehicleResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateVehicleResponse"/> class.
        /// </summary>
        /// <param name="output">Output of the use case.</param>
        public CreateVehicleResponse(CreateVehicleOutput output)
        {
            ArgumentNullException.ThrowIfNull(output);

            VehicleId = output.VehicleId;
            LicensePlate = output.LicensePlate;
            Brand = output.Brand;
            Model = output.Model;
            ManufacturingDate = output.ManufacturingDate;
        }

        /// <summary>
        /// Gets the vehicle identifier.
        /// </summary>
        [Required]
        public Guid VehicleId { get; }

        /// <summary>
        /// Gets the license plate.
        /// </summary>
        [Required]
        public string LicensePlate { get; }

        /// <summary>
        /// Gets the brand.
        /// </summary>
        [Required]
        public string Brand { get; }

        /// <summary>
        /// Gets the model.
        /// </summary>
        [Required]
        public string Model { get; }

        /// <summary>
        /// Gets the manufacturing date.
        /// </summary>
        [Required]
        public DateOnly ManufacturingDate { get; }
    }
}
