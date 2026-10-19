using System;
using System.ComponentModel.DataAnnotations;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.GetAvailableVehicles;

namespace GtMotive.Estimate.Microservice.Api.UseCases.GetAvailableVehicles
{
    /// <summary>
    /// Vehicle details returned by the available vehicles endpoint.
    /// </summary>
    public sealed class AvailableVehicleResponse
    {
        /// <summary>Initializes a new instance of the <see cref="AvailableVehicleResponse"/> class.</summary>
        /// <param name="output">Vehicle output.</param>
        public AvailableVehicleResponse(AvailableVehicleOutput output)
        {
            ArgumentNullException.ThrowIfNull(output);
            VehicleId = output.VehicleId;
            LicensePlate = output.LicensePlate;
            Brand = output.Brand;
            Model = output.Model;
            ManufacturingDate = output.ManufacturingDate;
        }

        /// <summary>Gets the vehicle identifier.</summary>
        [Required]
        public Guid VehicleId { get; }

        /// <summary>Gets the license plate.</summary>
        [Required]
        public string LicensePlate { get; }

        /// <summary>Gets the brand.</summary>
        [Required]
        public string Brand { get; }

        /// <summary>Gets the model.</summary>
        [Required]
        public string Model { get; }

        /// <summary>Gets the manufacturing date.</summary>
        [Required]
        public DateOnly ManufacturingDate { get; }
    }
}
