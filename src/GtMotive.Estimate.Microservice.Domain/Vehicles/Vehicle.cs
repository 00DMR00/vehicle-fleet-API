using System;

namespace GtMotive.Estimate.Microservice.Domain.Vehicles
{
    /// <summary>
    /// Aggregate root that represents a vehicle of the fleet.
    /// </summary>
    public sealed class Vehicle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Vehicle"/> class.
        /// </summary>
        /// <param name="id">Vehicle identifier.</param>
        /// <param name="licensePlate">License plate.</param>
        /// <param name="brand">Brand.</param>
        /// <param name="model">Model.</param>
        /// <param name="manufacturingDate">Manufacturing date, already validated against the fleet age rule.</param>
        /// <exception cref="DomainException">Brand or model are empty.</exception>
        public Vehicle(
            Guid id,
            string licensePlate,
            string brand,
            string model,
            ManufacturingDate manufacturingDate)
        {
            if (string.IsNullOrWhiteSpace(licensePlate))
            {
                throw new DomainException("The license plate is required.");
            }

            if (string.IsNullOrWhiteSpace(brand))
            {
                throw new DomainException("The brand is required.");
            }

            if (string.IsNullOrWhiteSpace(model))
            {
                throw new DomainException("The model is required.");
            }

            Id = id;
            LicensePlate = licensePlate.Trim().ToUpperInvariant();
            Brand = brand.Trim().ToUpperInvariant();
            Model = model.Trim().ToUpperInvariant();
            ManufacturingDate = manufacturingDate;
        }

        /// <summary>
        /// Gets the vehicle identifier.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets the license plate.
        /// </summary>
        public string LicensePlate { get; private set; }

        /// <summary>
        /// Gets the brand.
        /// </summary>
        public string Brand { get; private set; }

        /// <summary>
        /// Gets the model.
        /// </summary>
        public string Model { get; private set; }

        /// <summary>
        /// Gets the manufacturing date.
        /// </summary>
        public ManufacturingDate ManufacturingDate { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the vehicle is currently rented.
        /// </summary>
        public bool IsRented { get; private set; }

        /// <summary>
        /// Rebuilds a vehicle from its stored state, without applying the entry rules.
        /// </summary>
        /// <param name="id">Vehicle identifier.</param>
        /// <param name="licensePlate">License plate.</param>
        /// <param name="brand">Brand.</param>
        /// <param name="model">Model.</param>
        /// <param name="manufacturingDate">Stored manufacturing date.</param>
        /// <param name="isRented">Stored rental state.</param>
        /// <returns>The vehicle.</returns>
        public static Vehicle Restore(
            Guid id,
            string licensePlate,
            string brand,
            string model,
            DateOnly manufacturingDate,
            bool isRented)
        {
            return new Vehicle(id, licensePlate, brand, model, ManufacturingDate.FromPersisted(manufacturingDate))
            {
                IsRented = isRented,
            };
        }

        /// <summary>
        /// Marks the vehicle as rented.
        /// </summary>
        /// <exception cref="DomainException">The vehicle is already rented.</exception>
        public void Rent()
        {
            if (IsRented)
            {
                throw new DomainException("The vehicle " + LicensePlate + " is already rented.");
            }

            IsRented = true;
        }

        /// <summary>
        /// Marks the vehicle as available again.
        /// </summary>
        /// <exception cref="DomainException">The vehicle is not rented.</exception>
        public void Return()
        {
            if (!IsRented)
            {
                throw new DomainException("The vehicle " + LicensePlate + " is not rented.");
            }

            IsRented = false;
        }
    }
}
