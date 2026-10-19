using System;
using FluentAssertions;
using GtMotive.Estimate.Microservice.Domain;
using GtMotive.Estimate.Microservice.Domain.Vehicles;
using Xunit;

namespace GtMotive.Estimate.Microservice.UnitTests.Domain.Vehicles
{
    public sealed class VehicleTests
    {
        private static readonly DateOnly Today = new(2026, 10, 19);

        [Fact]
        public void IsCreatedAvailable()
        {
            var vehicle = CreateVehicle();

            vehicle.IsRented.Should().BeFalse();
        }

        [Fact]
        public void RentMarksTheVehicleAsRented()
        {
            var vehicle = CreateVehicle();

            vehicle.Rent();

            vehicle.IsRented.Should().BeTrue();
        }

        [Fact]
        public void RentThrowsWhenAlreadyRented()
        {
            var vehicle = CreateVehicle();
            vehicle.Rent();

            var act = vehicle.Rent;

            act.Should().Throw<DomainException>();
        }

        [Fact]
        public void ReturnMakesTheVehicleAvailableAgain()
        {
            var vehicle = CreateVehicle();
            vehicle.Rent();

            vehicle.Return();

            vehicle.IsRented.Should().BeFalse();
        }

        [Fact]
        public void ReturnThrowsWhenNotRented()
        {
            var vehicle = CreateVehicle();

            var act = vehicle.Return;

            act.Should().Throw<DomainException>();
        }

        [Fact]
        public void ThrowsWhenBrandIsEmpty()
        {
            var act = () => new Vehicle(
                Guid.NewGuid(),
                "1234ABC",
                " ",
                "Ibiza",
                new ManufacturingDate(Today.AddYears(-1), Today));

            act.Should().Throw<DomainException>();
        }

        [Fact]
        public void RestoreAcceptsAVehicleOlderThanFiveYearsAndKeepsItsState()
        {
            var vehicle = Vehicle.Restore(
                Guid.NewGuid(),
                "1234ABC",
                "Seat",
                "Ibiza",
                Today.AddYears(-8),
                true);

            vehicle.IsRented.Should().BeTrue();
        }

        private static Vehicle CreateVehicle() => new(
            Guid.NewGuid(),
            "1234ABC",
            "Seat",
            "Ibiza",
            new ManufacturingDate(Today.AddYears(-1), Today));
    }
}
