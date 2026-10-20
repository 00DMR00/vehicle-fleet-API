using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Vehicles;

namespace GtMotive.Estimate.Microservice.FunctionalTests.Infrastructure.InMemory
{
    internal sealed class InMemoryVehicleRepository : IVehicleRepository
    {
        private readonly ConcurrentDictionary<Guid, Vehicle> _vehicles = new();

        public Task Add(Vehicle vehicle)
        {
            _vehicles[vehicle.Id] = vehicle;

            return Task.CompletedTask;
        }

        public Task<Vehicle> GetById(Guid id)
        {
            _vehicles.TryGetValue(id, out var vehicle);

            return Task.FromResult(vehicle);
        }

        public Task<IReadOnlyCollection<Vehicle>> GetByRentalStatus(bool? isRented) =>
            Task.FromResult<IReadOnlyCollection<Vehicle>>([.. _vehicles.Values.Where(v => !isRented.HasValue || v.IsRented == isRented.Value)]);

        public Task Update(Vehicle vehicle)
        {
            _vehicles.TryUpdate(vehicle.Id, vehicle, _vehicles.GetValueOrDefault(vehicle.Id));

            return Task.CompletedTask;
        }

        public Task<bool> ExistsByLicensePlate(string licensePlate) =>
            Task.FromResult(_vehicles.Values.Any(v => v.LicensePlate == licensePlate));
    }
}
