using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Interfaces;

namespace GtMotive.Estimate.Microservice.FunctionalTests.Infrastructure.InMemory
{
    internal sealed class InMemoryUnitOfWork : IUnitOfWork
    {
        public Task<int> Save() => Task.FromResult(0);
    }
}
