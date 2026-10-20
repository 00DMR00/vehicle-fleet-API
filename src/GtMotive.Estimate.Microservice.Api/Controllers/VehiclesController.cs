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
        /// Lists vehicles, optionally filtered by rental status.
        /// </summary>
        /// <param name="isRented">True for rented vehicles, false for available vehicles, or null for all vehicles.</param>
        /// <returns>The matching vehicles.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyCollection<AvailableVehicleResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] bool? isRented = null)
        {
            _appLogger.LogDebug("Entering Get vehicles. IsRented: {IsRented}", isRented);

            var presenter = await _mediator.Send(new GetAvailableVehiclesRequest(isRented));

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
