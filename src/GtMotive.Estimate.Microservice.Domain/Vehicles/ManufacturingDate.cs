using System;
using System.Globalization;

namespace GtMotive.Estimate.Microservice.Domain.Vehicles
{
    /// <summary>
    /// Value object that represents the manufacturing date of a vehicle.
    /// A vehicle older than <see cref="MaxAgeInYears"/> years cannot be part of the fleet.
    /// </summary>
    public readonly struct ManufacturingDate
    {
        /// <summary>
        /// Maximum age, in years, a vehicle can have to be accepted in the fleet.
        /// </summary>
        public const int MaxAgeInYears = 5;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManufacturingDate"/> struct.
        /// </summary>
        /// <param name="date">Manufacturing date of the vehicle.</param>
        /// <param name="today">Current date. It is received as a parameter so the rule does not depend on the system clock.</param>
        /// <exception cref="DomainException">The date is in the future or older than the maximum allowed age.</exception>
        public ManufacturingDate(DateOnly date, DateOnly today)
        {
            if (date > today)
            {
                throw new DomainException(
                    "The manufacturing date " + Format(date) + " cannot be in the future.");
            }

            var oldestAllowed = today.AddYears(-MaxAgeInYears);

            if (date < oldestAllowed)
            {
                throw new DomainException(
                    "The manufacturing date " + Format(date) + " is older than " + MaxAgeInYears + " years.");
            }

            Value = date;
        }

        private ManufacturingDate(DateOnly value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets the manufacturing date.
        /// </summary>
        public DateOnly Value { get; }

        /// <summary>
        /// Rebuilds a manufacturing date that was already accepted into the fleet.
        /// The age rule is not applied: it only governs the entry of new vehicles.
        /// </summary>
        /// <param name="value">Stored manufacturing date.</param>
        /// <returns>The manufacturing date.</returns>
        public static ManufacturingDate FromPersisted(DateOnly value) => new(value);

        private static string Format(DateOnly date) => date.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
    }
}
