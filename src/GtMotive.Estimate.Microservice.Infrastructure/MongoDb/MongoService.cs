using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDb.Documents;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDb.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GtMotive.Estimate.Microservice.Infrastructure.MongoDb
{
    public class MongoService
    {
        public MongoService(IOptions<MongoDbSettings> options)
        {
            MongoClient = new MongoClient(options.Value.ConnectionString);
            Database = MongoClient.GetDatabase(options.Value.MongoDbDatabaseName);
        }

        public MongoClient MongoClient { get; }

        public IMongoDatabase Database { get; }

        /// <summary>
        /// Creates the unique indexes that guarantee the business rules under concurrency.
        /// </summary>
        /// <returns>A task that completes when the indexes exist.</returns>
        public async Task EnsureIndexes()
        {
            var vehicles = Database.GetCollection<VehicleDocument>(MongoCollectionNames.Vehicles);

            await vehicles.Indexes.CreateOneAsync(new CreateIndexModel<VehicleDocument>(
                Builders<VehicleDocument>.IndexKeys.Ascending(v => v.LicensePlate),
                new CreateIndexOptions { Unique = true, Name = "ux_vehicles_license_plate" }));

            var rentals = Database.GetCollection<RentalDocument>(MongoCollectionNames.Rentals);
            var onlyActive = Builders<RentalDocument>.Filter.Eq(r => r.IsActive, true);

            await rentals.Indexes.CreateManyAsync(
            [
                new CreateIndexModel<RentalDocument>(
                    Builders<RentalDocument>.IndexKeys.Ascending(r => r.CustomerId),
                    new CreateIndexOptions<RentalDocument>
                    {
                        Unique = true,
                        PartialFilterExpression = onlyActive,
                        Name = "ux_rentals_active_customer",
                    }),
                new CreateIndexModel<RentalDocument>(
                    Builders<RentalDocument>.IndexKeys.Ascending(r => r.VehicleId),
                    new CreateIndexOptions<RentalDocument>
                    {
                        Unique = true,
                        PartialFilterExpression = onlyActive,
                        Name = "ux_rentals_active_vehicle",
                    }),
            ]);
        }
    }
}
