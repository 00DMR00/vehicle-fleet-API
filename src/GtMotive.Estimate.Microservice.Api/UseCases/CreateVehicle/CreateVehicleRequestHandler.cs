using System;
using System.Threading;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.CreateVehicle;
using MediatR;

namespace GtMotive.Estimate.Microservice.Api.UseCases.CreateVehicle
{
    /// <summary>
    /// Builds the input of the create vehicle use case and runs it.
    /// </summary>
    /// <param name="useCase">Use case.</param>
    /// <param name="presenter">Presenter that builds the HTTP response.</param>
    public sealed class CreateVehicleRequestHandler(
        ICreateVehicleUseCase useCase,
        CreateVehiclePresenter presenter) : IRequestHandler<CreateVehicleRequest, IWebApiPresenter>
    {
        private readonly ICreateVehicleUseCase _useCase = useCase;
        private readonly CreateVehiclePresenter _presenter = presenter;

        /// <inheritdoc />
        public async Task<IWebApiPresenter> Handle(CreateVehicleRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            var input = new CreateVehicleInput(
                request.LicensePlate,
                request.Brand,
                request.Model,
                request.ManufacturingDate.GetValueOrDefault());

            await _useCase.Execute(input);

            return _presenter;
        }
    }
}
