using Insurance.Domain.RiskIndicators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Domain.Buildings
{
    public interface IBuildingRepository
    {
        Task<Building> GetByIdAsync(Guid id, CancellationToken ct);
        Task AddAsync(
        Building building,
        IReadOnlyCollection<RiskIndicatorType> riskIndicators,
        CancellationToken ct);

        Task UpdateAsync(
            Building building,
            CancellationToken ct);


    }
}
