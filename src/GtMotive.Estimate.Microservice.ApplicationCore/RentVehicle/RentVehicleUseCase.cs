using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Domain.Rentals;
using GtMotive.Estimate.Microservice.Domain.Vehicles;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.RentVehicle
{
    /// <summary>
    /// Rents an available vehicle to a person.
    /// </summary>
    /// <param name="vehicleRepository">Vehicle repository.</param>
    /// <param name="rentalService">Domain service with the rental rules.</param>
    /// <param name="unitOfWork">Unit of work.</param>
    /// <param name="timeProvider">Source of the current moment.</param>
    /// <param name="outputPort">Output port.</param>
    public sealed class RentVehicleUseCase(
        IVehicleRepository vehicleRepository,
        RentalService rentalService,
        IUnitOfWork unitOfWork,
        TimeProvider timeProvider,
        IRentVehicleOutputPort outputPort) : IRentVehicleUseCase
    {
        private readonly IVehicleRepository _vehicleRepository = vehicleRepository;
        private readonly RentalService _rentalService = rentalService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly TimeProvider _timeProvider = timeProvider;
        private readonly IRentVehicleOutputPort _outputPort = outputPort;

        /// <inheritdoc />
        public async Task Execute(RentVehicleInput input)
        {
            ArgumentNullException.ThrowIfNull(input);

            var vehicle = await _vehicleRepository.GetById(input.VehicleId);

            if (vehicle is null)
            {
                _outputPort.NotFoundHandle("The vehicle " + input.VehicleId + " does not exist.");
                return;
            }

            try
            {
                var rental = await _rentalService.Rent(vehicle, input.CustomerId, _timeProvider.GetUtcNow());
                await _unitOfWork.Save();

                _outputPort.StandardHandle(new RentVehicleOutput(rental));
            }
            catch (DomainException ex)
            {
                _outputPort.BusinessRuleViolationHandle(ex.Message);
            }
        }
    }
}
