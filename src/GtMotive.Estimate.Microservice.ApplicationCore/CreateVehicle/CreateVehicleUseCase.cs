using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Domain.Vehicles;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.CreateVehicle
{
    /// <summary>
    /// Adds a new vehicle to the fleet.
    /// </summary>
    /// <param name="vehicleRepository">Vehicle repository.</param>
    /// <param name="unitOfWork">Unit of work.</param>
    /// <param name="timeProvider">Source of the current date.</param>
    /// <param name="outputPort">Output port.</param>
    public sealed class CreateVehicleUseCase(
        IVehicleRepository vehicleRepository,
        IUnitOfWork unitOfWork,
        TimeProvider timeProvider,
        ICreateVehicleOutputPort outputPort) : ICreateVehicleUseCase
    {
        private readonly IVehicleRepository _vehicleRepository = vehicleRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly TimeProvider _timeProvider = timeProvider;
        private readonly ICreateVehicleOutputPort _outputPort = outputPort;

        /// <inheritdoc />
        public async Task Execute(CreateVehicleInput input)
        {
            ArgumentNullException.ThrowIfNull(input);

            try
            {
                var today = DateOnly.FromDateTime(_timeProvider.GetUtcNow().UtcDateTime);
                var manufacturingDate = new ManufacturingDate(input.ManufacturingDate, today);
                var vehicle = new Vehicle(
                    Guid.NewGuid(),
                    input.LicensePlate,
                    input.Brand,
                    input.Model,
                    manufacturingDate);

                if (await _vehicleRepository.ExistsByLicensePlate(vehicle.LicensePlate))
                {
                    _outputPort.BusinessRuleViolationHandle(
                        "A vehicle with license plate " + vehicle.LicensePlate + " already exists.");
                    return;
                }

                await _vehicleRepository.Add(vehicle);
                await _unitOfWork.Save();

                _outputPort.StandardHandle(new CreateVehicleOutput(vehicle));
            }
            catch (DomainException ex)
            {
                _outputPort.BusinessRuleViolationHandle(ex.Message);
            }
        }
    }
}
