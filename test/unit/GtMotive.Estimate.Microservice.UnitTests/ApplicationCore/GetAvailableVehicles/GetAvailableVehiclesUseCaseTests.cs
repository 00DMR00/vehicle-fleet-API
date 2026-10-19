using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.GetAvailableVehicles;
using GtMotive.Estimate.Microservice.Domain.Vehicles;
using Moq;
using Xunit;

namespace GtMotive.Estimate.Microservice.UnitTests.ApplicationCore.GetAvailableVehicles
{
    public sealed class GetAvailableVehiclesUseCaseTests
    {
        private static readonly DateOnly Today = new(2026, 10, 19);

        private readonly Mock<IVehicleRepository> _vehicleRepository = new();

        [Fact]
        public async Task ReturnsAvailableVehiclesFromTheRepository()
        {
            var vehicle = new Vehicle(
                Guid.NewGuid(),
                "1234ABC",
                "Seat",
                "Ibiza",
                new ManufacturingDate(Today.AddYears(-1), Today));
            _vehicleRepository
                .Setup(repository => repository.GetAvailable())
                .ReturnsAsync([vehicle]);
            var useCase = new GetAvailableVehiclesUseCase(_vehicleRepository.Object);

            var result = await useCase.Execute();

            var output = Assert.Single(result);
            Assert.Equal(vehicle.Id, output.VehicleId);
            Assert.Equal(vehicle.LicensePlate, output.LicensePlate);
            Assert.Equal(vehicle.Brand, output.Brand);
            Assert.Equal(vehicle.Model, output.Model);
            Assert.Equal(vehicle.ManufacturingDate.Value, output.ManufacturingDate);
            _vehicleRepository.Verify(repository => repository.GetAvailable(), Times.Once);
        }

        [Fact]
        public async Task ReturnsAnEmptyCollectionWhenNoVehiclesAreAvailable()
        {
            _vehicleRepository
                .Setup(repository => repository.GetAvailable())
                .ReturnsAsync([]);
            var useCase = new GetAvailableVehiclesUseCase(_vehicleRepository.Object);

            var result = await useCase.Execute();

            Assert.Empty(result);
        }
    }
}
