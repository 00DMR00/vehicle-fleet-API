using MediatR;

namespace GtMotive.Estimate.Microservice.Api.UseCases.GetAvailableVehicles
{
    /// <summary>
    /// Request to retrieve the available vehicles in the fleet.
    /// </summary>
    public sealed class GetAvailableVehiclesRequest(bool? isRented = null) : IRequest<IWebApiPresenter>
    {
        public bool? IsRented { get; } = isRented;
    }
}
