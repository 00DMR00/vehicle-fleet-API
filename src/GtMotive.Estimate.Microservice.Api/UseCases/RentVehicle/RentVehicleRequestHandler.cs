using System;
using System.Threading;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.RentVehicle;
using MediatR;

namespace GtMotive.Estimate.Microservice.Api.UseCases.RentVehicle
{
    /// <summary>
    /// Builds the input of the rent vehicle use case and runs it.
    /// </summary>
    /// <param name="useCase">Use case.</param>
    /// <param name="presenter">Presenter that builds the HTTP response.</param>
    public sealed class RentVehicleRequestHandler(
        IRentVehicleUseCase useCase,
        RentVehiclePresenter presenter) : IRequestHandler<RentVehicleRequest, IWebApiPresenter>
    {
        private readonly IRentVehicleUseCase _useCase = useCase;
        private readonly RentVehiclePresenter _presenter = presenter;

        /// <inheritdoc />
        public async Task<IWebApiPresenter> Handle(RentVehicleRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            var input = new RentVehicleInput(request.VehicleId.GetValueOrDefault(), request.CustomerId);

            await _useCase.Execute(input);

            return _presenter;
        }
    }
}
