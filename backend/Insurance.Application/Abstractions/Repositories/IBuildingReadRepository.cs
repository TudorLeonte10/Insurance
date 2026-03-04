using Insurance.Application.Buildings.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Abstractions.Repositories
{
    public interface IBuildingReadRepository
    {
        Task<BuildingDetailsDto?> GetByIdAsync(
            Guid buildingId,
            CancellationToken ct);

        Task<IReadOnlyList<BuildingDetailsDto>> GetByClientIdAsync(
            Guid clientId,
            CancellationToken ct);

        Task<BuildingGeoContextDto?> GetGeoContextAsync(
            Guid buildingId,
            CancellationToken cancellationToken);

        Task<bool> IsOwnedByClientAsync(Guid buildingId, Guid clientId, CancellationToken cancellationToken);

        Task<BuildingAnomalyContextDto?> GetAnomalyContextAsync(Guid buildingId, CancellationToken cancellationToken);
    }
}
