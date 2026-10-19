using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.CreateVehicle;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.UseCases.CreateVehicle
{
    /// <summary>
    /// Converts the outcome of the create vehicle use case into an HTTP response.
    /// </summary>
    public sealed class CreateVehiclePresenter : IWebApiPresenter, ICreateVehicleOutputPort
    {
        /// <inheritdoc />
        public IActionResult ActionResult { get; private set; }

        /// <inheritdoc />
        public void StandardHandle(CreateVehicleOutput response)
        {
            ActionResult = new ObjectResult(new CreateVehicleResponse(response))
            {
                StatusCode = StatusCodes.Status201Created,
            };
        }

        /// <inheritdoc />
        public void BusinessRuleViolationHandle(string message)
        {
            ActionResult = new BadRequestObjectResult(new ProblemDetails
            {
                Title = "Business rule violation",
                Detail = message,
                Status = StatusCodes.Status400BadRequest,
            });
        }
    }
}
