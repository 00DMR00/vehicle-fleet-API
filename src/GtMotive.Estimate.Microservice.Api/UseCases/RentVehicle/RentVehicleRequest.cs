using System;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace GtMotive.Estimate.Microservice.Api.UseCases.RentVehicle
{
    /// <summary>
    /// Request to rent a vehicle.
    /// </summary>
    public sealed class RentVehicleRequest : IRequest<IWebApiPresenter>
    {
        /// <summary>
        /// Gets or sets the identifier of the vehicle to rent.
        /// </summary>
        [Required]
        public Guid? VehicleId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the person who rents.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string CustomerId { get; set; }
    }
}
