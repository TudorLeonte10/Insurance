using AutoMapper;
using AutoMapper.QueryableExtensions;
using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Policy.DTOs;
using Insurance.Domain.Buildings;
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

    }

}
