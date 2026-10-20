using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain;
using GtMotive.Estimate.Microservice.Domain.Vehicles;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDb.Documents;
using MongoDB.Driver;

namespace GtMotive.Estimate.Microservice.Infrastructure.MongoDb.Repositories
{
    internal sealed class VehicleRepository(MongoService mongoService) : IVehicleRepository
    {
        private readonly IMongoCollection<VehicleDocument> _vehicles =
            mongoService.Database.GetCollection<VehicleDocument>(MongoCollectionNames.Vehicles);

        public async Task Add(Vehicle vehicle)
        {
            try
            {
                await _vehicles.InsertOneAsync(VehicleDocument.FromDomain(vehicle));
            }
            catch (MongoWriteException ex) when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
            {
                throw new DomainException(
                    "A vehicle with license plate " + vehicle.LicensePlate + " already exists.",
                    ex);
            }
        }

        public async Task<Vehicle> GetById(Guid id)
        {
            var document = await _vehicles.Find(v => v.Id == id).FirstOrDefaultAsync();

            return document?.ToDomain();
        }

        public async Task<IReadOnlyCollection<Vehicle>> GetByRentalStatus(bool? isRented)
        {
            var filter = isRented.HasValue
                ? Builders<VehicleDocument>.Filter.Eq(v => v.IsRented, isRented.Value)
                : Builders<VehicleDocument>.Filter.Empty;
            var documents = await _vehicles.Find(filter).ToListAsync();

            return documents.ConvertAll(v => v.ToDomain());
        }

        public Task Update(Vehicle vehicle) =>
            _vehicles.ReplaceOneAsync(v => v.Id == vehicle.Id, VehicleDocument.FromDomain(vehicle));

        public Task<bool> ExistsByLicensePlate(string licensePlate) =>
            _vehicles.Find(v => v.LicensePlate == licensePlate).AnyAsync();
    }
}
