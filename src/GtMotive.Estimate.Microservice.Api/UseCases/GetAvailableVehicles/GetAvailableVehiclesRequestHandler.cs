using System.Threading;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.GetAvailableVehicles;
using MediatR;

namespace GtMotive.Estimate.Microservice.Api.UseCases.GetAvailableVehicles
{
    /// <summary>
    /// Executes the available vehicles query and returns its presenter.
    /// </summary>
    /// <param name="useCase">Query use case.</param>
    /// <param name="presenter">Presenter that builds the HTTP response.</param>
    public sealed class GetAvailableVehiclesRequestHandler(
        IGetAvailableVehiclesUseCase useCase,
        GetAvailableVehiclesPresenter presenter) : IRequestHandler<GetAvailableVehiclesRequest, IWebApiPresenter>
    {
        private readonly IGetAvailableVehiclesUseCase _useCase = useCase;
        private readonly GetAvailableVehiclesPresenter _presenter = presenter;

        /// <inheritdoc />
        public async Task<IWebApiPresenter> Handle(GetAvailableVehiclesRequest request, CancellationToken cancellationToken)
        {
            var vehicles = await _useCase.Execute();
            _presenter.Handle(vehicles);
            return _presenter;
        }
    }
}
