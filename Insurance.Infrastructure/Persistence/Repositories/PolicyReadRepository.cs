using AutoMapper;
using AutoMapper.QueryableExtensions;
using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Policy.DTOs;
using Insurance.Domain.Buildings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Repositories
{
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

        public IQueryable<PolicyReportReadModel> GetQueryData()
        {
            return _db.Policies.
                AsNoTracking()
                .Select(p => new PolicyReportReadModel
                {
                    PolicyId = p.Id,
                    PolicyStartDate = p.StartDate,
                    Country = p.Building.City.County.Country.Name,
                    County = p.Building.City.County.Name,
                    City = p.Building.City.Name,
                    CityId = p.Building.City.Id,
                    BrokerCode = p.Broker.BrokerCode,
                    BrokerName = p.Broker.Name,
                    CurrencyCode = p.Currency.Code,
                    FinalPremium = p.FinalPremium,
                    FinalPremiumBase = p.FinalPremium * p.Currency.ExchangeRateToBase,
                    Status = p.Status,
                    BuildingType = p.Building.Type
                });
        }
    }

}
