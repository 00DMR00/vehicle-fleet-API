using System;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using GtMotive.Estimate.Microservice.InfrastructureTests.Infrastructure;
using Xunit;

namespace GtMotive.Estimate.Microservice.InfrastructureTests.Specs
{
    [Collection(TestCollections.TestServer)]
    public sealed class VehiclesControllerTests(GenericInfrastructureTestServerFixture fixture)
    {
        private static readonly Uri VehiclesUri = new("/api/vehicles", UriKind.Relative);

        private readonly GenericInfrastructureTestServerFixture _fixture = fixture;

        [Fact]
        public async Task GetReturnsAvailableVehicles()
        {
            using var client = _fixture.Server.CreateClient();

            using var response = await client.GetAsync(VehiclesUri);
            using var content = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.RootElement.ValueKind.Should().Be(JsonValueKind.Array);
            content.RootElement.GetArrayLength().Should().Be(0);
        }

        [Fact]
        public async Task PostWithMissingFieldsReturnsBadRequest()
        {
            using var client = _fixture.Server.CreateClient();

            using var response = await client.PostAsJsonAsync(VehiclesUri, new { licensePlate = "1234ABC" });

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task PostWithValidBodyReachesTheUseCaseAndReturnsCreated()
        {
            using var client = _fixture.Server.CreateClient();
            var body = new
            {
                licensePlate = "1234ABC",
                brand = "Seat",
                model = "Ibiza",
                manufacturingDate = "2025-01-15",
            };

            using var response = await client.PostAsJsonAsync(VehiclesUri, body);
            var content = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            content.Should().Contain("1234ABC");
        }
    }
}
