using AutoMapper;
using AutoMapper.QueryableExtensions;
using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Policy.DTOs;
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
            var entity = await _db.Policies
                .AsNoTracking()
                .FirstAsync(p => p.Id == policyId, ct);

            return _mapper.Map<PolicyDetailsDto>(entity);
        }
    }

}
