using System;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.CreateVehicle
{
    /// <summary>
    /// Input message of the create vehicle use case.
    /// </summary>
    /// <param name="licensePlate">License plate.</param>
    /// <param name="brand">Brand.</param>
    /// <param name="model">Model.</param>
    /// <param name="manufacturingDate">Manufacturing date.</param>
    public sealed class CreateVehicleInput(
        string licensePlate,
        string brand,
        string model,
        DateOnly manufacturingDate) : IUseCaseInput
    {
        /// <summary>
        /// Gets the license plate.
        /// </summary>
        public string LicensePlate { get; } = licensePlate;

        /// <summary>
        /// Gets the brand.
        /// </summary>
        public string Brand { get; } = brand;

        /// <summary>
        /// Gets the model.
        /// </summary>
        public string Model { get; } = model;

        /// <summary>
        /// Gets the manufacturing date.
        /// </summary>
        public DateOnly ManufacturingDate { get; } = manufacturingDate;
    }
}
