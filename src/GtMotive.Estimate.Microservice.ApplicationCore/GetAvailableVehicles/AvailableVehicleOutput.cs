using System;
using GtMotive.Estimate.Microservice.Domain.Vehicles;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.GetAvailableVehicles
{
    /// <summary>
    /// Vehicle details returned by the available vehicles query.
    /// </summary>
    public sealed class AvailableVehicleOutput
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AvailableVehicleOutput"/> class.
        /// </summary>
        /// <param name="vehicle">Available vehicle.</param>
        public AvailableVehicleOutput(Vehicle vehicle)
        {
            ArgumentNullException.ThrowIfNull(vehicle);

            VehicleId = vehicle.Id;
            LicensePlate = vehicle.LicensePlate;
            Brand = vehicle.Brand;
            Model = vehicle.Model;
            ManufacturingDate = vehicle.ManufacturingDate.Value;
        }

        /// <summary>Gets the vehicle identifier.</summary>
        public Guid VehicleId { get; }

        /// <summary>Gets the license plate.</summary>
        public string LicensePlate { get; }

        /// <summary>Gets the brand.</summary>
        public string Brand { get; }

        /// <summary>Gets the model.</summary>
        public string Model { get; }

        /// <summary>Gets the manufacturing date.</summary>
        public DateOnly ManufacturingDate { get; }
    }
}
