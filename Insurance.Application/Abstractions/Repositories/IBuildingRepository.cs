using Insurance.Application.Buildings.DTOs;
using Insurance.Domain.Buildings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Abstractions.Repositories
{
    public interface IBuildingRepository
    {  
        Task<IReadOnlyList<Building>> GetAllBuildingsByClientId(Guid clientId, CancellationToken cancellationToken);
        //Task<BuildingDetailsDto> GetBuildingById()
    }
}
