using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using GtMotive.Estimate.Microservice.Api.UseCases;
using GtMotive.Estimate.Microservice.Api.UseCases.CreateVehicle;
using GtMotive.Estimate.Microservice.Api.UseCases.GetAvailableVehicles;
using GtMotive.Estimate.Microservice.Api.UseCases.RentVehicle;
using GtMotive.Estimate.Microservice.FunctionalTests.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace GtMotive.Estimate.Microservice.FunctionalTests.Specs
{
    [Collection(TestCollections.Functional)]
    public sealed class RentalFlowTests(CompositionRootTestFixture fixture)
    {
        private readonly CompositionRootTestFixture _fixture = fixture;

        [Fact]
        public async Task RentsAnAvailableVehicle()
        {
            var vehicleId = await CreateVehicle(RecentDate());

            var result = await Rent(vehicleId, UniqueCustomer());

            result.StatusCode.Should().Be(StatusCodes.Status201Created);
            result.Value.Should().BeOfType<RentVehicleResponse>();
        }

        [Fact]
        public async Task ListsOnlyVehiclesThatAreNotRented()
        {
            var availableVehicleId = await CreateVehicle(RecentDate());
            var rentedVehicleId = await CreateVehicle(RecentDate());
            await Rent(rentedVehicleId, UniqueCustomer());

            IReadOnlyCollection<AvailableVehicleResponse> vehicles = null;
            await _fixture.UsingHandlerForRequestResponse<GetAvailableVehiclesRequest, IWebApiPresenter>(async handler =>
            {
                var presenter = await handler.Handle(new GetAvailableVehiclesRequest(), CancellationToken.None);
                var result = presenter.ActionResult.Should().BeAssignableTo<OkObjectResult>().Subject;
                vehicles = result.Value.Should()
                    .BeAssignableTo<IReadOnlyCollection<AvailableVehicleResponse>>()
                    .Subject;
            });

            vehicles.Should().ContainSingle(vehicle => vehicle.VehicleId == availableVehicleId);
            vehicles.Should().NotContain(vehicle => vehicle.VehicleId == rentedVehicleId);
        }

        [Fact]
        public async Task RejectsASecondRentalForTheSameCustomer()
        {
            var customer = UniqueCustomer();
            var firstVehicle = await CreateVehicle(RecentDate());
            var secondVehicle = await CreateVehicle(RecentDate());

            var first = await Rent(firstVehicle, customer);
            var second = await Rent(secondVehicle, customer);

            first.StatusCode.Should().Be(StatusCodes.Status201Created);
            second.StatusCode.Should().Be(StatusCodes.Status409Conflict);
        }

        [Fact]
        public async Task ReportsNotFoundWhenTheVehicleDoesNotExist()
        {
            var result = await Rent(Guid.NewGuid(), UniqueCustomer());

            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task RejectsAVehicleOlderThanFiveYears()
        {
            var request = NewVehicleRequest(RecentDate().AddYears(-6));
            ObjectResult result = null;

            await _fixture.UsingHandlerForRequestResponse<CreateVehicleRequest, IWebApiPresenter>(async handler =>
            {
                var presenter = await handler.Handle(request, CancellationToken.None);
                result = presenter.ActionResult.Should().BeAssignableTo<ObjectResult>().Subject;
            });

            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        private static DateOnly RecentDate() =>
            DateOnly.FromDateTime(TimeProvider.System.GetUtcNow().UtcDateTime).AddYears(-1);

        private static string UniqueCustomer() =>
            "customer-" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

        private static CreateVehicleRequest NewVehicleRequest(DateOnly manufacturingDate) => new()
        {
            LicensePlate = "T" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture),
            Brand = "Seat",
            Model = "Ibiza",
            ManufacturingDate = manufacturingDate,
        };

        private async Task<Guid> CreateVehicle(DateOnly manufacturingDate)
        {
            var request = NewVehicleRequest(manufacturingDate);
            var vehicleId = Guid.Empty;

            await _fixture.UsingHandlerForRequestResponse<CreateVehicleRequest, IWebApiPresenter>(async handler =>
            {
                var presenter = await handler.Handle(request, CancellationToken.None);
                var result = presenter.ActionResult.Should().BeAssignableTo<ObjectResult>().Subject;

                result.StatusCode.Should().Be(StatusCodes.Status201Created);
                vehicleId = result.Value.Should().BeOfType<CreateVehicleResponse>().Subject.VehicleId;
            });

            return vehicleId;
        }

        private async Task<ObjectResult> Rent(Guid vehicleId, string customerId)
        {
            var request = new RentVehicleRequest { VehicleId = vehicleId, CustomerId = customerId };
            ObjectResult result = null;

            await _fixture.UsingHandlerForRequestResponse<RentVehicleRequest, IWebApiPresenter>(async handler =>
            {
                var presenter = await handler.Handle(request, CancellationToken.None);
                result = presenter.ActionResult.Should().BeAssignableTo<ObjectResult>().Subject;
            });

            return result;
        }
    }
}
