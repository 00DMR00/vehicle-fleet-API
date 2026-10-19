using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.CreateVehicle;
using GtMotive.Estimate.Microservice.Domain.Vehicles;

namespace GtMotive.Estimate.Microservice.InfrastructureTests.Infrastructure
{
    /// <summary>
    /// Replaces the real use case so the test stays at host level.
    /// </summary>
    /// <param name="outputPort">Output port.</param>
    internal sealed class CreateVehicleUseCaseStub(ICreateVehicleOutputPort outputPort) : ICreateVehicleUseCase
    {
        private readonly ICreateVehicleOutputPort _outputPort = outputPort;

        public Task Execute(CreateVehicleInput input)
        {
            ArgumentNullException.ThrowIfNull(input);

            var vehicle = new Vehicle(
                Guid.NewGuid(),
                input.LicensePlate,
                input.Brand,
                input.Model,
                ManufacturingDate.FromPersisted(input.ManufacturingDate));

            _outputPort.StandardHandle(new CreateVehicleOutput(vehicle));

            return Task.CompletedTask;
        }
    }
}
