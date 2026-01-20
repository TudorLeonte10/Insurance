using Insurance.Application.Buildings.DTOs;
using Insurance.Domain.Buildings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Abstractions.Repositories
{
    public interface IBuildingRepository
    {  
        Task<IReadOnlyList<Building>> GetAllBuildingsByClientIdAsync(Guid clientId, CancellationToken cancellationToken);
        Task<Building?> GetBuildingByIdAsync(Guid buildingId, CancellationToken cancellationToken);
        Task AddBuildingAsync(Building building, CancellationToken ct);
        void UpdateBuilding(Building building, CancellationToken ct);
    }
}
