using System;
using System.Threading.Tasks;
using FluentAssertions;
using GtMotive.Estimate.Microservice.Domain;
using GtMotive.Estimate.Microservice.Domain.Rentals;
using GtMotive.Estimate.Microservice.Domain.Vehicles;
using Moq;
using Xunit;

namespace GtMotive.Estimate.Microservice.UnitTests.Domain.Rentals
{
    public sealed class RentalServiceTests
    {
        private const string CustomerId = "12345678Z";

        private static readonly DateOnly Today = new(2026, 10, 19);
        private static readonly DateTimeOffset Now = new(2026, 10, 19, 10, 0, 0, TimeSpan.Zero);

        private readonly Mock<IVehicleRepository> _vehicleRepository = new();
        private readonly Mock<IRentalRepository> _rentalRepository = new();

        [Fact]
        public async Task RentsTheVehicleAndStoresTheRental()
        {
            var vehicle = CreateVehicle();
            _rentalRepository.Setup(r => r.ExistsActiveForCustomer(CustomerId)).ReturnsAsync(false);
            var service = CreateService();

            var rental = await service.Rent(vehicle, CustomerId, Now);

            vehicle.IsRented.Should().BeTrue();
            rental.VehicleId.Should().Be(vehicle.Id);
            rental.IsActive.Should().BeTrue();
            _rentalRepository.Verify(r => r.Add(rental), Times.Once);
            _vehicleRepository.Verify(r => r.Update(vehicle), Times.Once);
        }

        [Fact]
        public async Task ThrowsWhenTheCustomerAlreadyHasAnActiveRental()
        {
            var vehicle = CreateVehicle();
            _rentalRepository.Setup(r => r.ExistsActiveForCustomer(CustomerId)).ReturnsAsync(true);
            var service = CreateService();

            Func<Task> act = () => service.Rent(vehicle, CustomerId, Now);
            await act.Should().ThrowAsync<DomainException>();
            vehicle.IsRented.Should().BeFalse();
            _rentalRepository.Verify(r => r.Add(It.IsAny<Rental>()), Times.Never);
            _vehicleRepository.Verify(r => r.Update(It.IsAny<Vehicle>()), Times.Never);
        }

        private static Vehicle CreateVehicle() => new(
            Guid.NewGuid(),
            "1234ABC",
            "Seat",
            "Ibiza",
            new ManufacturingDate(Today.AddYears(-1), Today));

        private RentalService CreateService() => new(_vehicleRepository.Object, _rentalRepository.Object);
    }
}
