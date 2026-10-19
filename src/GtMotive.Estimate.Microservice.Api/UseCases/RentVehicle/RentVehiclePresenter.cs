using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.RentVehicle;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.UseCases.RentVehicle
{
    /// <summary>
    /// Converts the outcome of the rent vehicle use case into an HTTP response.
    /// </summary>
    public sealed class RentVehiclePresenter : IWebApiPresenter, IRentVehicleOutputPort
    {
        /// <inheritdoc />
        public IActionResult ActionResult { get; private set; }

        /// <inheritdoc />
        public void StandardHandle(RentVehicleOutput response)
        {
            ActionResult = new ObjectResult(new RentVehicleResponse(response))
            {
                StatusCode = StatusCodes.Status201Created,
            };
        }

        /// <inheritdoc />
        public void NotFoundHandle(string message)
        {
            ActionResult = new NotFoundObjectResult(new ProblemDetails
            {
                Title = "Vehicle not found",
                Detail = message,
                Status = StatusCodes.Status404NotFound,
            });
        }

        /// <inheritdoc />
        public void BusinessRuleViolationHandle(string message)
        {
            ActionResult = new ConflictObjectResult(new ProblemDetails
            {
                Title = "Business rule violation",
                Detail = message,
                Status = StatusCodes.Status409Conflict,
            });
        }
    }
}
