using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Interfaces;

namespace GtMotive.Estimate.Microservice.Infrastructure.MongoDb
{
    /// <summary>
    /// MongoDB standalone has no multi-document transactions, so each repository writes immediately
    /// and there is nothing pending to flush. It exists to keep the use case contract of the template.
    /// </summary>
    internal sealed class MongoUnitOfWork : IUnitOfWork
    {
        public Task<int> Save() => Task.FromResult(0);
    }
}
