using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Api.UseCases.RentVehicle;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Controllers
{
    /// <summary>
    /// Manages the rentals of the fleet.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public sealed class RentalsController(IMediator mediator, IAppLogger<RentalsController> appLogger) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly IAppLogger<RentalsController> _appLogger = appLogger;

        /// <summary>
        /// Rents a vehicle to a person.
        /// </summary>
        /// <param name="request">Vehicle and person.</param>
        /// <returns>The created rental.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(RentVehicleResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Post([FromBody][Required] RentVehicleRequest request)
        {
            _appLogger.LogDebug("Entering Post. Request: {@request}", request);

            var presenter = await _mediator.Send(request);

            return presenter.ActionResult;
        }
    }
}
