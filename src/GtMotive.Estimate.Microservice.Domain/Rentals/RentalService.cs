using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Vehicles;

namespace GtMotive.Estimate.Microservice.Domain.Rentals
{
    /// <summary>
    /// Domain service with the rental rules that involve more than one aggregate.
    /// It never saves changes: that belongs to the use case through the unit of work.
    /// </summary>
    /// <param name="vehicleRepository">Vehicle repository.</param>
    /// <param name="rentalRepository">Rental repository.</param>
    public sealed class RentalService(IVehicleRepository vehicleRepository, IRentalRepository rentalRepository)
    {
        private readonly IVehicleRepository _vehicleRepository = vehicleRepository;
        private readonly IRentalRepository _rentalRepository = rentalRepository;

        /// <summary>
        /// Rents a vehicle to a customer, enforcing one active rental per person.
        /// </summary>
        /// <param name="vehicle">Vehicle to rent.</param>
        /// <param name="customerId">Identifier of the person who rents.</param>
        /// <param name="now">Current moment.</param>
        /// <returns>The new rental.</returns>
        /// <exception cref="DomainException">The customer already has an active rental or the vehicle is not available.</exception>
        public async Task<Rental> Rent(Vehicle vehicle, string customerId, DateTimeOffset now)
        {
            ArgumentNullException.ThrowIfNull(vehicle);

            var rental = Rental.Start(vehicle.Id, customerId, now);

            if (await _rentalRepository.ExistsActiveForCustomer(rental.CustomerId))
            {
                throw new DomainException("The customer " + rental.CustomerId + " already has an active rental.");
            }

            vehicle.Rent();

            await _rentalRepository.Add(rental);
            await _vehicleRepository.Update(vehicle);

            return rental;
        }
    }
}
