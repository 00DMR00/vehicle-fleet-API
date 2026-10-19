using System;

namespace GtMotive.Estimate.Microservice.Domain.Rentals
{
    /// <summary>
    /// Entity that represents a customer holding a vehicle during a period of time.
    /// </summary>
    public sealed class Rental
    {
        private Rental(Guid id, Guid vehicleId, string customerId, DateTimeOffset startDate)
        {
            Id = id;
            VehicleId = vehicleId;
            CustomerId = customerId;
            StartDate = startDate;
        }

        /// <summary>
        /// Gets the rental identifier.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets the identifier of the rented vehicle.
        /// </summary>
        public Guid VehicleId { get; private set; }

        /// <summary>
        /// Gets the identifier of the person who rents the vehicle.
        /// </summary>
        public string CustomerId { get; private set; }

        /// <summary>
        /// Gets the moment the rental started.
        /// </summary>
        public DateTimeOffset StartDate { get; private set; }

        /// <summary>
        /// Gets the moment the vehicle was returned, or null while the rental is active.
        /// </summary>
        public DateTimeOffset? EndDate { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the rental is still active.
        /// </summary>
        public bool IsActive => EndDate is null;

        /// <summary>
        /// Starts a new rental.
        /// </summary>
        /// <param name="vehicleId">Identifier of the rented vehicle.</param>
        /// <param name="customerId">Identifier of the person who rents.</param>
        /// <param name="startDate">Moment the rental starts.</param>
        /// <returns>The new active rental.</returns>
        /// <exception cref="DomainException">The customer identifier is empty.</exception>
        public static Rental Start(Guid vehicleId, string customerId, DateTimeOffset startDate)
        {
            return string.IsNullOrWhiteSpace(customerId)
                ? throw new DomainException("The customer is required.")
                : new Rental(Guid.NewGuid(), vehicleId, customerId.Trim(), startDate);
        }

        /// <summary>
        /// Rebuilds a rental from its stored state.
        /// </summary>
        /// <param name="id">Rental identifier.</param>
        /// <param name="vehicleId">Identifier of the rented vehicle.</param>
        /// <param name="customerId">Identifier of the person who rents.</param>
        /// <param name="startDate">Moment the rental started.</param>
        /// <param name="endDate">Moment the vehicle was returned, or null.</param>
        /// <returns>The rental.</returns>
        public static Rental Restore(
            Guid id,
            Guid vehicleId,
            string customerId,
            DateTimeOffset startDate,
            DateTimeOffset? endDate)
        {
            return new Rental(id, vehicleId, customerId, startDate)
            {
                EndDate = endDate,
            };
        }

        /// <summary>
        /// Closes the rental.
        /// </summary>
        /// <param name="endDate">Moment the vehicle is returned.</param>
        /// <exception cref="DomainException">The rental is already closed or the end is before the start.</exception>
        public void Close(DateTimeOffset endDate)
        {
            if (!IsActive)
            {
                throw new DomainException("The rental is already closed.");
            }

            if (endDate < StartDate)
            {
                throw new DomainException("The end date cannot be before the start date.");
            }

            EndDate = endDate;
        }
    }
}
