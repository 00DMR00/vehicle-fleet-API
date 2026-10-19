using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Rentals;

namespace GtMotive.Estimate.Microservice.FunctionalTests.Infrastructure.InMemory
{
    internal sealed class InMemoryRentalRepository : IRentalRepository
    {
        private readonly ConcurrentBag<Rental> _rentals = [];

        public Task Add(Rental rental)
        {
            _rentals.Add(rental);

            return Task.CompletedTask;
        }

        public Task<bool> ExistsActiveForCustomer(string customerId) =>
            Task.FromResult(_rentals.Any(r => r.CustomerId == customerId && r.IsActive));
    }
}
