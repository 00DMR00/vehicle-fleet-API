using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.RentVehicle;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Domain.Rentals;
using GtMotive.Estimate.Microservice.Domain.Vehicles;
using Moq;
using Xunit;

namespace GtMotive.Estimate.Microservice.UnitTests.ApplicationCore.RentVehicle
{
    public sealed class RentVehicleUseCaseTests
    {
        private const string CustomerId = "12345678Z";

        private static readonly DateTimeOffset Now = new(2026, 10, 19, 10, 0, 0, TimeSpan.Zero);
        private static readonly DateOnly Today = new(2026, 10, 19);

        private readonly Mock<IVehicleRepository> _vehicleRepository = new();
        private readonly Mock<IRentalRepository> _rentalRepository = new();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly Mock<TimeProvider> _timeProvider = new();
        private readonly Mock<IRentVehicleOutputPort> _outputPort = new();

        [Fact]
        public async Task RentsTheVehicleAndSavesChanges()
        {
            var vehicle = CreateVehicle();
            _vehicleRepository.Setup(r => r.GetById(vehicle.Id)).ReturnsAsync(vehicle);
            _rentalRepository.Setup(r => r.ExistsActiveForCustomer(CustomerId)).ReturnsAsync(false);
            var useCase = CreateUseCase();

            await useCase.Execute(new RentVehicleInput(vehicle.Id, CustomerId));

            _unitOfWork.Verify(u => u.Save(), Times.Once);
            _outputPort.Verify(p => p.StandardHandle(It.IsAny<RentVehicleOutput>()), Times.Once);
            _outputPort.Verify(p => p.NotFoundHandle(It.IsAny<string>()), Times.Never);
            _outputPort.Verify(p => p.BusinessRuleViolationHandle(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task ReportsNotFoundWhenTheVehicleDoesNotExist()
        {
            var vehicleId = Guid.NewGuid();
            _vehicleRepository.Setup(r => r.GetById(vehicleId)).ReturnsAsync((Vehicle)null);
            var useCase = CreateUseCase();

            await useCase.Execute(new RentVehicleInput(vehicleId, CustomerId));

            _outputPort.Verify(p => p.NotFoundHandle(It.IsAny<string>()), Times.Once);
            _unitOfWork.Verify(u => u.Save(), Times.Never);
        }

        [Fact]
        public async Task ReportsABusinessRuleViolationWhenTheCustomerAlreadyHasARental()
        {
            var vehicle = CreateVehicle();
            _vehicleRepository.Setup(r => r.GetById(vehicle.Id)).ReturnsAsync(vehicle);
            _rentalRepository.Setup(r => r.ExistsActiveForCustomer(CustomerId)).ReturnsAsync(true);
            var useCase = CreateUseCase();

            await useCase.Execute(new RentVehicleInput(vehicle.Id, CustomerId));

            _outputPort.Verify(p => p.BusinessRuleViolationHandle(It.IsAny<string>()), Times.Once);
            _outputPort.Verify(p => p.StandardHandle(It.IsAny<RentVehicleOutput>()), Times.Never);
            _unitOfWork.Verify(u => u.Save(), Times.Never);
        }

        private static Vehicle CreateVehicle() => new(
            Guid.NewGuid(),
            "1234ABC",
            "Seat",
            "Ibiza",
            new ManufacturingDate(Today.AddYears(-1), Today));

        private RentVehicleUseCase CreateUseCase()
        {
            _timeProvider.Setup(t => t.GetUtcNow()).Returns(Now);

            return new RentVehicleUseCase(
                _vehicleRepository.Object,
                new RentalService(_vehicleRepository.Object, _rentalRepository.Object),
                _unitOfWork.Object,
                _timeProvider.Object,
                _outputPort.Object);
        }
    }
}
