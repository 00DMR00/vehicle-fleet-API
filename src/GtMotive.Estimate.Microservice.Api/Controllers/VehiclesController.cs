using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Api.UseCases.CreateVehicle;
using GtMotive.Estimate.Microservice.Api.UseCases.GetAvailableVehicles;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Controllers
{
    /// <summary>
    /// Manages the vehicles of the fleet.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public sealed class VehiclesController(IMediator mediator, IAppLogger<VehiclesController> appLogger) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly IAppLogger<VehiclesController> _appLogger = appLogger;

        /// <summary>
        /// Lists vehicles that are currently available for rent.
        /// </summary>
        /// <returns>The available vehicles.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyCollection<AvailableVehicleResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            _appLogger.LogDebug("Entering Get available vehicles.");

            var presenter = await _mediator.Send(new GetAvailableVehiclesRequest());

            return presenter.ActionResult;
        }

        /// <summary>
        /// Adds a vehicle to the fleet.
        /// </summary>
        /// <param name="request">Vehicle data.</param>
        /// <returns>The created vehicle.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(CreateVehicleResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody][Required] CreateVehicleRequest request)
        {
            _appLogger.LogDebug("Entering Post. Request: {@request}", request);

            var presenter = await _mediator.Send(request);

            return presenter.ActionResult;
        }
    }
}
