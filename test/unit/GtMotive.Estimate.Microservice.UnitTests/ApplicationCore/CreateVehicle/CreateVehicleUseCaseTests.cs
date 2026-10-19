using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.CreateVehicle;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Domain.Vehicles;
using Moq;
using Xunit;

namespace GtMotive.Estimate.Microservice.UnitTests.ApplicationCore.CreateVehicle
{
    public sealed class CreateVehicleUseCaseTests
    {
        private static readonly DateTimeOffset Now = new(2026, 10, 19, 10, 0, 0, TimeSpan.Zero);
        private static readonly DateOnly Today = new(2026, 10, 19);

        private readonly Mock<IVehicleRepository> _vehicleRepository = new();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly Mock<TimeProvider> _timeProvider = new();
        private readonly Mock<ICreateVehicleOutputPort> _outputPort = new();

        [Fact]
        public async Task CreatesTheVehicleAndSavesChanges()
        {
            var useCase = CreateUseCase();
            var input = new CreateVehicleInput("1234abc", "Seat", "Ibiza", Today.AddYears(-1));

            await useCase.Execute(input);

            _vehicleRepository.Verify(r => r.Add(It.Is<Vehicle>(v => v.LicensePlate == "1234ABC")), Times.Once);
            _unitOfWork.Verify(u => u.Save(), Times.Once);
            _outputPort.Verify(p => p.StandardHandle(It.IsAny<CreateVehicleOutput>()), Times.Once);
            _outputPort.Verify(p => p.BusinessRuleViolationHandle(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task ReportsABusinessRuleViolationWhenTheVehicleIsTooOld()
        {
            var useCase = CreateUseCase();
            var input = new CreateVehicleInput("1234ABC", "Seat", "Ibiza", Today.AddYears(-6));

            await useCase.Execute(input);

            _outputPort.Verify(p => p.BusinessRuleViolationHandle(It.IsAny<string>()), Times.Once);
            _vehicleRepository.Verify(r => r.Add(It.IsAny<Vehicle>()), Times.Never);
            _unitOfWork.Verify(u => u.Save(), Times.Never);
        }

        [Fact]
        public async Task ReportsABusinessRuleViolationWhenTheLicensePlateAlreadyExists()
        {
            _vehicleRepository.Setup(r => r.ExistsByLicensePlate("1234ABC")).ReturnsAsync(true);
            var useCase = CreateUseCase();
            var input = new CreateVehicleInput("1234ABC", "Seat", "Ibiza", Today.AddYears(-1));

            await useCase.Execute(input);

            _outputPort.Verify(p => p.BusinessRuleViolationHandle(It.IsAny<string>()), Times.Once);
            _vehicleRepository.Verify(r => r.Add(It.IsAny<Vehicle>()), Times.Never);
            _unitOfWork.Verify(u => u.Save(), Times.Never);
        }

        private CreateVehicleUseCase CreateUseCase()
        {
            _timeProvider.Setup(t => t.GetUtcNow()).Returns(Now);

            return new CreateVehicleUseCase(
                _vehicleRepository.Object,
                _unitOfWork.Object,
                _timeProvider.Object,
                _outputPort.Object);
        }
    }
}
