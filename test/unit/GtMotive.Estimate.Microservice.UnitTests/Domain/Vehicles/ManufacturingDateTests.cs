using System;
using FluentAssertions;
using GtMotive.Estimate.Microservice.Domain;
using GtMotive.Estimate.Microservice.Domain.Vehicles;
using Xunit;

namespace GtMotive.Estimate.Microservice.UnitTests.Domain.Vehicles
{
    public sealed class ManufacturingDateTests
    {
        private static readonly DateOnly Today = new(2026, 10, 19);

        [Fact]
        public void CreatesWhenVehicleIsTwoYearsOld()
        {
            var date = Today.AddYears(-2);

            var manufacturingDate = new ManufacturingDate(date, Today);

            manufacturingDate.Value.Should().Be(date);
        }

        [Fact]
        public void CreatesWhenVehicleIsExactlyFiveYearsOld()
        {
            var date = Today.AddYears(-5);

            var manufacturingDate = new ManufacturingDate(date, Today);

            manufacturingDate.Value.Should().Be(date);
        }

        [Fact]
        public void ThrowsWhenVehicleIsOneDayOlderThanFiveYears()
        {
            var date = Today.AddYears(-5).AddDays(-1);

            var act = () => { _ = new ManufacturingDate(date, Today); };

            act.Should().Throw<DomainException>();
        }

        [Fact]
        public void ThrowsWhenVehicleIsSixYearsOld()
        {
            var date = Today.AddYears(-6);

            var act = () => { _ = new ManufacturingDate(date, Today); };

            act.Should().Throw<DomainException>();
        }

        [Fact]
        public void ThrowsWhenDateIsInTheFuture()
        {
            var date = Today.AddDays(1);

            var act = () => { _ = new ManufacturingDate(date, Today); };

            act.Should().Throw<DomainException>();
        }
    }
}
