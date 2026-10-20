using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Vehicles;
using Microsoft.Extensions.DependencyInjection;

namespace GtMotive.Estimate.Microservice.Host.Infrastructure
{
    /// <summary>
    /// Inserts a small, repeatable set of vehicles when the application starts.
    /// </summary>
    internal static class VehicleDataSeeder
    {
        private static readonly (string LicensePlate, string Brand, string Model, DateOnly ManufacturingDate)[] Vehicles =
        [
            ("1234ABC", "Toyota", "Corolla", new DateOnly(2022, 2, 10)),
            ("2345BCD", "Seat", "Leon", new DateOnly(2023, 5, 18)),
            ("3456CDE", "Volkswagen", "Golf", new DateOnly(2024, 8, 22)),
            ("4567DEF", "Renault", "Clio", new DateOnly(2025, 1, 15)),
            ("5678EFG", "Peugeot", "308", new DateOnly(2022, 11, 4)),
            ("6789FGH", "Ford", "Focus", new DateOnly(2023, 9, 12)),
            ("7890GHJ", "Hyundai", "i30", new DateOnly(2025, 3, 7)),
            ("8901HJK", "Kia", "Ceed", new DateOnly(2022, 6, 29)),
            ("9012JKL", "Skoda", "Octavia", new DateOnly(2024, 2, 19)),
            ("0123KLM", "Dacia", "Sandero", new DateOnly(2023, 12, 1)),
        ];

        public static async Task Seed(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IVehicleRepository>();
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            foreach (var (licensePlate, brand, model, manufacturingDate) in Vehicles)
            {
                if (await repository.ExistsByLicensePlate(licensePlate))
                {
                    continue;
                }

                var vehicle = new Vehicle(
                    Guid.NewGuid(),
                    licensePlate,
                    brand,
                    model,
                    new ManufacturingDate(manufacturingDate, today));

                await repository.Add(vehicle);
            }
        }
    }
}
