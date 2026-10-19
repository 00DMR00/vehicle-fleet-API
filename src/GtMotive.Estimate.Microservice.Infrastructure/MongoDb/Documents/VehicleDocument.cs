using System;
using GtMotive.Estimate.Microservice.Domain.Vehicles;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GtMotive.Estimate.Microservice.Infrastructure.MongoDb.Documents
{
    internal sealed class VehicleDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        public string LicensePlate { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }

        public DateTime ManufacturingDate { get; set; }

        public bool IsRented { get; set; }

        public static VehicleDocument FromDomain(Vehicle vehicle) => new()
        {
            Id = vehicle.Id,
            LicensePlate = vehicle.LicensePlate,
            Brand = vehicle.Brand,
            Model = vehicle.Model,
            ManufacturingDate = vehicle.ManufacturingDate.Value.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc),
            IsRented = vehicle.IsRented,
        };

        public Vehicle ToDomain() => Vehicle.Restore(
            Id,
            LicensePlate,
            Brand,
            Model,
            DateOnly.FromDateTime(ManufacturingDate),
            IsRented);
    }
}
