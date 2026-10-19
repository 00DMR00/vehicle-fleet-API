using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain;
using GtMotive.Estimate.Microservice.Domain.Rentals;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDb.Documents;
using MongoDB.Driver;

namespace GtMotive.Estimate.Microservice.Infrastructure.MongoDb.Repositories
{
    internal sealed class RentalRepository(MongoService mongoService) : IRentalRepository
    {
        private readonly IMongoCollection<RentalDocument> _rentals =
            mongoService.Database.GetCollection<RentalDocument>(MongoCollectionNames.Rentals);

        public async Task Add(Rental rental)
        {
            try
            {
                await _rentals.InsertOneAsync(RentalDocument.FromDomain(rental));
            }
            catch (MongoWriteException ex) when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
            {
                throw new DomainException(
                    "The customer already has an active rental or the vehicle is already rented.",
                    ex);
            }
        }

        public Task<bool> ExistsActiveForCustomer(string customerId) =>
            _rentals.Find(r => r.CustomerId == customerId && r.IsActive).AnyAsync();
    }
}
