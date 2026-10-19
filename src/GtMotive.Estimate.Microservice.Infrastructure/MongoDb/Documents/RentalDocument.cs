using System;
using GtMotive.Estimate.Microservice.Domain.Rentals;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GtMotive.Estimate.Microservice.Infrastructure.MongoDb.Documents
{
    internal sealed class RentalDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        [BsonRepresentation(BsonType.String)]
        public Guid VehicleId { get; set; }

        public string CustomerId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsActive { get; set; }

        public static RentalDocument FromDomain(Rental rental) => new()
        {
            Id = rental.Id,
            VehicleId = rental.VehicleId,
            CustomerId = rental.CustomerId,
            StartDate = rental.StartDate.UtcDateTime,
            EndDate = rental.EndDate?.UtcDateTime,
            IsActive = rental.IsActive,
        };

        public Rental ToDomain()
        {
            DateTimeOffset? endDate = EndDate is null ? null : ToOffset(EndDate.Value);

            return Rental.Restore(Id, VehicleId, CustomerId, ToOffset(StartDate), endDate);
        }

        private static DateTimeOffset ToOffset(DateTime utc) =>
            new(DateTime.SpecifyKind(utc, DateTimeKind.Utc));
    }
}
