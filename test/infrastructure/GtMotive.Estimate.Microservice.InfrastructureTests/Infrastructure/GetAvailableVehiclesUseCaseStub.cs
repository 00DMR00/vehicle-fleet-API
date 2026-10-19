using System.Collections.Generic;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.GetAvailableVehicles;

namespace GtMotive.Estimate.Microservice.InfrastructureTests.Infrastructure
{
    internal sealed class GetAvailableVehiclesUseCaseStub : IGetAvailableVehiclesUseCase
    {
        public Task<IReadOnlyCollection<AvailableVehicleOutput>> Execute() =>
            Task.FromResult<IReadOnlyCollection<AvailableVehicleOutput>>([]);
    }
}
