using Insurance.Application.Abstractions;
using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Authentication;
using Insurance.Application.Buildings.Commands;
using Insurance.Application.Exceptions;
using Insurance.Domain.Buildings;
using Insurance.Domain.Clients;
using Insurance.Domain.Exceptions;
using MediatR;

namespace Insurance.Application.Buildings.Commands
{
    public class CreateBuildingCommandHandler
        : IRequestHandler<CreateBuildingCommand, Guid>
    {
        private readonly IBuildingRepository _buildingRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IGeographyReadRepository _geographyRepository;
        private readonly IUnitOfWork _uow;
        private readonly ICurrentUserContext _currentUserContext;

        public CreateBuildingCommandHandler(
            IBuildingRepository buildingRepository,
            IClientRepository clientRepository,
            IGeographyReadRepository geographyRepository,
            IUnitOfWork uow,
            ICurrentUserContext currentUserContext)
        {
            _buildingRepository = buildingRepository;
            _clientRepository = clientRepository;
            _geographyRepository = geographyRepository;
            _uow = uow;
            _currentUserContext = currentUserContext;
        }

        public async Task<Guid> Handle(
            CreateBuildingCommand request,
            CancellationToken cancellationToken)
        {
          
            await ValidateAsync(request, cancellationToken);

            var dto = request.BuildingDto;

            var building = Building.Create(
                request.ClientId,
                dto.CityId,
                dto.Type,
                dto.Street,
                dto.Number,
                dto.ConstructionYear,
                dto.NumberOfFloors,
                dto.SurfaceArea,
                dto.InsuredValue);

            await _buildingRepository.AddAsync(
                building,
                dto.RiskIndicators.Distinct().ToList(),
                cancellationToken);

            await _uow.SaveChangesAsync(cancellationToken);

            return building.Id;
        }

        private async Task ValidateAsync(CreateBuildingCommand request, CancellationToken ct)
        {
            var brokerId = _currentUserContext.BrokerId;

            var client = await _clientRepository.GetByIdAsync(request.ClientId, ct);
            if (client is null)
                throw new NotFoundException($"Client with id {request.ClientId} not found");

            if (client.BrokerId != brokerId)
                throw new ForbiddenException($"The broker is not allowed to create a building for this client");

            var city = await _geographyRepository.GetCityByIdAsync(request.BuildingDto.CityId, ct);
            if (city is null)
                throw new NotFoundException($"City with id {request.BuildingDto.CityId} not found");
        }
    }
 }

