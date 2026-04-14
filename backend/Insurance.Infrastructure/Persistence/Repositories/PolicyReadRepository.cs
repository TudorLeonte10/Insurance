using AutoMapper;
using AutoMapper.QueryableExtensions;
using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Common.Paging;
using Insurance.Application.Policy.DTOs;
using Insurance.Domain.Buildings;
using Insurance.Domain.Policies;
using Insurance.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Repositories
{
    [ExcludeFromCodeCoverage]
    public class PolicyReadRepository : IPolicyReadRepository
    {
        private readonly InsuranceDbContext _db;
        private readonly IMapper _mapper;

        public PolicyReadRepository(InsuranceDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<PolicyDetailsDto?> GetByIdAsync(
            Guid policyId,
            CancellationToken ct)
        {
            return await _db.Policies
                .AsNoTracking()
                .Where(p => p.Id == policyId)
                .ProjectTo<PolicyDetailsDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(ct);
        }

        public async Task<int> GetPoliciesOfClientFromLastYearAsync(Guid clientId, CancellationToken cancellationToken)
        {
            return await _db.Policies.Where(p => p.ClientId == clientId && p.CreatedAt >= DateTime.UtcNow.AddYears(-1))
                .CountAsync(cancellationToken);
        }

        public async Task<decimal> GetBrokerAveragePremiumAsync(Guid brokerId, CancellationToken cancellationToken)
        {
            var premiums = _db.Policies
              .Where(p => p.BrokerId == brokerId)
              .Select(p => (decimal?)p.FinalPremium);

            var average = await premiums.AverageAsync(cancellationToken);

            return average ?? 0m;
        }

        public async Task<decimal> GetClientAverageInsuredValue(Guid clientId, CancellationToken cancellationToken)
        {
            var insuredValues = _db.Policies
                .Where(p => p.ClientId == clientId && p.CreatedAt >= DateTime.UtcNow.AddYears(-5))
                .Select(p => (decimal?)p.Building.InsuredValue);

            var average = await insuredValues.AverageAsync(cancellationToken);

            return average ?? 0m;
        }

        public async Task<decimal> GetClientAveragePremiumRatioAsync(Guid clientId, CancellationToken cancellationToken)
        {
            var premiumRatios = _db.Policies
                .Where(p => p.ClientId == clientId && p.CreatedAt >= DateTime.UtcNow.AddYears(-5))
                .Select(p => (decimal?)(p.FinalPremium / p.Building.InsuredValue));

            var average = await premiumRatios.AverageAsync(cancellationToken);

            return average ?? 0m;
        }

        public async Task<decimal> GetBrokerGlobalAveragePremiumAsync(CancellationToken cancellationToken)
        {
            var premiums = _db.Policies
                .Select(p => (decimal?)p.FinalPremium);

            var average = await premiums.AverageAsync(cancellationToken);

            return average ?? 0m;
        }

        public async Task<decimal> GetClientGlobalAverageInsuredValue(CancellationToken cancellationToken)
        {
            var insuredValues = _db.Policies
                .Select(p => (decimal?)p.Building.InsuredValue);

            var average = await insuredValues.AverageAsync(cancellationToken);

            return average ?? 0m;
        }

        public async Task<PagedResult<PolicyDetailsDto>> GetPoliciesToReviewAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var items = await _db.Policies
                 .AsNoTracking()
                 .Where(p => p.Status == PolicyStatus.UnderReview)
                 .OrderBy(p => p.CreatedAt)
                 .Skip(pageSize * (pageNumber - 1))
                 .Take(pageSize)
                 .ProjectTo<PolicyDetailsDto>(_mapper.ConfigurationProvider)
                 .ToListAsync(cancellationToken);

            var totalCount = await _db.Policies.CountAsync(p => p.Status == PolicyStatus.UnderReview, cancellationToken);

            return new PagedResult<PolicyDetailsDto>(
                items,
                pageNumber,
                pageSize,
                totalCount);
        }

        public async Task<IEnumerable<PolicyByCityDto>> GetPolicyByCityAsync(Guid brokerId, CancellationToken cancellationToken)
        {
            var result = await _db.Policies
                .AsNoTracking()
                .Where(p => p.BrokerId == brokerId)
                .GroupBy(p => p.Building.City)
                .Select(g => new PolicyByCityDto
                {
                    City = g.Key.Name,
                    PolicyCount = g.Count()
                })
                .ToListAsync(cancellationToken);
            return result;
        }

    }
}
