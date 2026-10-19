using System.Collections.Generic;
using System.Linq;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.GetAvailableVehicles;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.UseCases.GetAvailableVehicles
{
    /// <summary>
    /// Converts the available vehicles query output into an HTTP response.
    /// </summary>
    public sealed class GetAvailableVehiclesPresenter : IWebApiPresenter
    {
        /// <inheritdoc />
        public IActionResult ActionResult { get; private set; }

        /// <summary>Builds the successful response.</summary>
        /// <param name="vehicles">Available vehicles.</param>
        public void Handle(IReadOnlyCollection<AvailableVehicleOutput> vehicles)
        {
            ActionResult = new OkObjectResult(vehicles.Select(vehicle => new AvailableVehicleResponse(vehicle)).ToList());
        }
    }
}
